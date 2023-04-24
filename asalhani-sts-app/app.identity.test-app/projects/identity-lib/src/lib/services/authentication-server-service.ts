import {Inject, Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {CONFIG_TOKEN_KEY, IdentityUiConfig} from "../utils/identity-ui-config";
import {UserLoginModel} from "../models/user-login-model";
import {Observable} from "rxjs";
import {UserLoginResult} from "../models/user-login-result";

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

  authenticate(loginInfo: UserLoginModel): Observable<UserLoginResult> {
    return this.http.post<UserLoginResult>(`${this.endpoint}/sign-in`, loginInfo, { withCredentials: true });
  }
}
