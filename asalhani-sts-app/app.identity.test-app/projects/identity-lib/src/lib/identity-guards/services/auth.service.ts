import {Inject, Injectable} from '@angular/core';
import {Log, User, UserManager} from "oidc-client";
import {IdentityGuardsConfig} from "../models/identity-guards-config";
import {LoginTypeEnum} from "../../models/login-type-enum";
import {BehaviorSubject, Observable, Subject} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private manager: UserManager;
  private user: User | null = null;
  private isInitialized: boolean = false;

  //  As soon as the user’s status changes, we want to inform any component that needs that kind of information
  private _loginChangedSubject = new BehaviorSubject<boolean>(false);
  private _loginChanged$ = this._loginChangedSubject.asObservable();

  getIsLoggedIn(): Observable<boolean> {
    return this._loginChanged$;
  }

  setIsLoggedIn(latestValue: boolean){
    return this._loginChangedSubject.next(latestValue);
  }
  constructor(
    @Inject('identity_guards_config') private _config: IdentityGuardsConfig,
  ) {
    if (_config.getConfigValues().enableConsoleLogging) {
      Log.logger = console;
      Log.level = Log.DEBUG;
    }
    this.manager = new UserManager(_config.getConfigValues().oidcSettings);

    // With the addAccessTokenExpired function, we subscribe to an event as soon as the access token expires.
    // this.manager.events.addAccessTokenExpired(_ => {
    //   this.setIsLoggedIn(false);
    // });

    this.fetchUser();
  }

  isLoggedIn(): Promise<boolean> {
    let isLoggedIn: boolean = false;
    return new Promise((resolve, reject) => {
      if (!this.isInitialized) {
        let timer = setInterval(() => {
          if (this.isInitialized) {
            clearInterval(timer);
            isLoggedIn = this.isLoggedInInternal();
            this.setIsLoggedIn(isLoggedIn);
            resolve(isLoggedIn);
          }
        }, 100);
      } else {
        isLoggedIn = this.isLoggedInInternal();
        this.setIsLoggedIn(isLoggedIn);
        resolve(isLoggedIn);
      }
    });
  }

  private fetchUser(): Promise<void> {
    return this.manager.getUser().then(user => {
      this.user = user;
      this.isInitialized = true;
    });
  }

  private isLoggedInInternal(): boolean {
    let isLoggedIn =this.user != null && !this.user.expired;
    this.setIsLoggedIn(isLoggedIn);
    return isLoggedIn;
  }

  getClaims(): any {
    return this.user?.profile;
  }

  getAuthorizationHeaderValueAsync(): Promise<string> {
    return this.fetchUser().then(() => {
      return this.getAuthorizationHeaderValue();
    });
  }

  getAuthorizationHeaderValue(): string {
    if (this.user) {
      return `${this.user.token_type} ${this.user.access_token}`;
    }
    return '';
  }

  startAuthentication(): Promise<void> {
    return this.manager.signinRedirect();
  }

  startAuthenticationCustom(loginType: string): Promise<void> {
    return this.manager.signinRedirect({
      extraQueryParams: {loginType: loginType} // you can add any number of custom key: value pairs
    });
  }

  completeAuthentication(): Promise<void> {
    return this.manager.signinRedirectCallback().then(user => {
      this.user = user;
      this.setIsLoggedIn(this.isLoggedInInternal());
    });
  }

  completeSilentRefresh() {
    this.manager.signinSilentCallback()
      .catch((err) => {
        console.log(err);
      });
  }

  signOut() {
    this.manager.signoutRedirect({'id_token_hint': this.user ? this.user.id_token : ''})
      .then(v => this.setIsLoggedIn(this.isLoggedInInternal()));
  }

  public finishLogout = () => {
    this.setIsLoggedIn(false);
    return this.manager.signoutRedirectCallback();
  };
}
