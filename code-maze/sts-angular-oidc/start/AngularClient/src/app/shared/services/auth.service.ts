import {User, UserManager, UserManagerSettings} from 'oidc-client';
import {Constants} from './constants';
import {Subject} from 'rxjs';
import {Injectable} from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private _userManager: UserManager;
  private _user: User;

  //  As soon as the user’s status changes, we want to inform any component that needs that kind of information
  private _loginChangedSubject = new Subject<boolean>();
  public loginChanged = this._loginChangedSubject.asObservable();

  private get idpSettings(): UserManagerSettings {
    return {
      authority: Constants.idpAuthority,
      client_id: Constants.clientId,
      redirect_uri: `${Constants.clientRoot}/signin-callback`,
      scope: 'openid profile companyApi',
      response_type: 'code',
      post_logout_redirect_uri: `${Constants.clientRoot}/signout-callback`,
      automaticSilentRenew: true,
      silent_redirect_uri: `${Constants.clientRoot}/assets/silent-callback.html`
    };
  }

  constructor() {
    this._userManager = new UserManager(this.idpSettings);

    // With the addAccessTokenExpired function, we subscribe to an event as soon as the access token expires.
    this._userManager.events.addAccessTokenExpired(_ => {
      this._loginChangedSubject.next(false);
    });
  }

  /*
   This function will redirect us to the authorization endpoint on the IDP server. Additionally, the
   UserManager stores a user result in the session storage after a successful login action and we can always
   retrieve that object and use all the information it contains.
   */
  public login = () => {
    return this._userManager.signinRedirect();
  };

  public isAuthenticated = (): Promise<boolean> => {
    return this._userManager.getUser()
      .then(user => {
        if (this._user !== user) {
          this._loginChangedSubject.next(this.checkUser(user));
        }

        this._user = user;
        return this.checkUser(user);
      });
  };

  /*
  we need to complete the signin process because we don’t want to navigate the user to the Not Found page. To
  do that, we have to process the response from the /authorization endpoint and populate the user object with
  the id and access tokens.

  signinRedirectCallback() function that processes the response from the /authorization endpoint and returns a
   promise.
   */
  public finishLogin = (): Promise<User> => {
    return this._userManager.signinRedirectCallback()
      .then(user => {
        this._user = user;
        this._loginChangedSubject.next(this.checkUser(user));
        return user;
      });
  };

  public getAccessToken = (): Promise<string> => {
    return this._userManager.getUser()
      .then(user => {
        return !!user && !user.expired ? user.access_token : null;
      });
  };

  // check if current user has admin role
  public checkIfUserIsAdmin = (): Promise<boolean> => {
    return this._userManager.getUser()
      .then(user => {
        return user?.profile.role === 'Admin';
      });
  };

  public logout = () => {
    this._userManager.signoutRedirect();
  };
  public finishLogout = () => {
    this._user = null;
    this._loginChangedSubject.next(false);
    return this._userManager.signoutRedirectCallback();
  };

  private checkUser = (user: User): boolean => {
    return !!user && !user.expired;
  };
}
