import {Inject, Injectable} from '@angular/core';
import {Log, User, UserManager} from "oidc-client";
import {IdentityGuardsConfig} from "../models/identity-guards-config";
import {LoginTypeEnum} from "../../models/login-type-enum";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private manager: UserManager;
  private user: User | null = null;
  private isInitialized: boolean = false;

  constructor(
    @Inject('identity_guards_config') private _config: IdentityGuardsConfig,
  ) {
    if (_config.getConfigValues().enableConsoleLogging) {
      Log.logger = console;
      Log.level = Log.DEBUG;
    }
    this.manager = new UserManager(_config.getConfigValues().oidcSettings);
    this.fetchUser();
  }

  isLoggedIn(): Promise<boolean> {
    return new Promise((resolve, reject) => {
      if (!this.isInitialized) {
        let timer = setInterval(() => {
          if (this.isInitialized) {
            clearInterval(timer);
            resolve(this.isLoggedInInternal());
          }
        }, 100);
      } else {
        resolve(this.isLoggedInInternal());
      }
    });
  }

  private fetchUser() : Promise<void> {
    return this.manager.getUser().then(user => {
      this.user = user;
      this.isInitialized = true;
    });
  }

  private isLoggedInInternal(): boolean {
    return this.user != null && !this.user.expired;
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
    });
  }

  completeSilentRefresh() {
    this.manager.signinSilentCallback()
      .catch((err) => {
        console.log(err);
      });
  }

  signOut() {
    this.manager.signoutRedirect({ 'id_token_hint': this.user ? this.user.id_token : '' });
  }
}
