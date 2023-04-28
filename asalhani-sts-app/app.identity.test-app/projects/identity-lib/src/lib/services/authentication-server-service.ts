import {Inject, Injectable} from "@angular/core";
import {HttpClient, HttpParams} from "@angular/common/http";
import {CONFIG_TOKEN_KEY, IdentityUiConfig} from "../utils/identity-ui-config";
import {Observable} from "rxjs";
import {UserLoginResult} from "../models/user-login-result";
import {UserLogoutResult} from "../models/user-logout-result";
import {EmployeeUserLoginModel, PublicUserLoginModel} from "../models/user-login-model";

@Injectable({
  providedIn: 'root'
})
export class AuthenticationServerService {

  private endpoint = 'authentication';

  constructor(private http: HttpClient, @Inject(CONFIG_TOKEN_KEY) _config: IdentityUiConfig) {

    const baseUrl = _config.getConfigValues().identityServerEndpoint;
    let baseHTTPUrl :string;
    baseHTTPUrl = baseUrl;
    if(baseHTTPUrl=="/")
      baseHTTPUrl="";

    this.endpoint = `${(baseHTTPUrl ? baseHTTPUrl : '')}/${this.endpoint}`;
  }

  authenticatePublicUser(loginInfo: PublicUserLoginModel | EmployeeUserLoginModel): Observable<UserLoginResult> {
    return this.http.post<UserLoginResult>(`${this.endpoint}/sign-in`, loginInfo, { withCredentials: true });
  }

  signOutAttempt(logoutId: string): Observable<UserLogoutResult> {
    const params = new HttpParams().set('logoutId', logoutId);
    return this.http.get<UserLogoutResult>(`${this.endpoint}/sign-out`, { params: params, withCredentials: true });
  }

  signOutAssured(logoutId: string): Observable<UserLogoutResult> {
    return this.http.post<UserLogoutResult>(`${this.endpoint}/sign-out`, logoutId, { withCredentials: true });
  }
}
