import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { UserLoginModel } from '../../models/user-login-model';
import { Observable } from 'rxjs';
import { UserLoginResult } from '../../models/user-login-result';
import { OtpModel } from '../../models/otp-model';
import { ForgotPasswordModel } from '../../models/forgot-password-model';
import { ForgotPasswordResult } from '../../models/forgot-password-result';
import { ResetPasswordModel } from '../../models/reset-password-model';
import { ResetPasswordResult } from '../../models/reset-password-result';
import { OtpVerificationResultResult } from '../../models/otp-verification-result';
import { IdentityUiConfig, CONFIG_TOKEN_KEY } from '../../utils/identity-ui-config';
import { UserLogoutResult } from '../../models/user-logout-result';
import { ChangeExpiredPasswordModel } from '../../models/change-expired-password-model';
import { ChangeExpiredPasswordResult } from '../../models/change-expired-password-result';
import { ResponseContentType } from '@angular/http';
import { OtpResendResult } from '../../models/otp-resend-result';

@Injectable({
  providedIn: 'root'
})

export class AuthenticationServerService {

  private endpoint = 'authentication';
  private captchaEndpoint = 'captcha';

  constructor(private http: HttpClient, @Inject(CONFIG_TOKEN_KEY) _config: IdentityUiConfig) {
    const baseUrl = _config.getConfigValues().identityServerEndpoint;
    let baseHTTPUrl :string;
    baseHTTPUrl = baseUrl;
    if(baseHTTPUrl=="/")
    baseHTTPUrl="";
    
    this.endpoint = `${(baseHTTPUrl ? baseHTTPUrl : '')}/${this.endpoint}`;
    this.captchaEndpoint = `${(baseHTTPUrl ? baseHTTPUrl : '')}/${this.captchaEndpoint}`;
  }

  authenticate(loginInfo: UserLoginModel): Observable<UserLoginResult> {
    return this.http.post<UserLoginResult>(`${this.endpoint}/sign-in`, loginInfo, { withCredentials: true });
  }

  verifyOtp(otpInfo: OtpModel): Observable<OtpVerificationResultResult> {
    return this.http.post<OtpVerificationResultResult>(`${this.endpoint}/verify-otp`, otpInfo, { withCredentials: true });
  }

  resendOtp(): Observable<OtpResendResult> {
    return this.http.post<OtpResendResult>(`${this.endpoint}/resend-otp`, null,{ withCredentials: true });
  }

  forgotPassword(forgotPasswordInfo: ForgotPasswordModel): Observable<ForgotPasswordResult> {
    return this.http.post<ForgotPasswordResult>(`${this.endpoint}/forgot-password`, forgotPasswordInfo,{ withCredentials: true });
  }

  validateEmailToken(token: string): Observable<boolean> {
    const params = new HttpParams().set('token', token);
    return this.http.get<boolean>(`${this.endpoint}/validate-email-token`, { params: params });
  }

  resetPassword(resetPasswordInfo: ResetPasswordModel): Observable<ResetPasswordResult> {
    return this.http.post<ResetPasswordResult>(`${this.endpoint}/reset-password`, resetPasswordInfo, { withCredentials: true });
  }

  signOutAttempt(logoutId: string): Observable<UserLogoutResult> {
    const params = new HttpParams().set('logoutId', logoutId);
    return this.http.get<UserLogoutResult>(`${this.endpoint}/sign-out`, { params: params, withCredentials: true });
  }

  signOutAssured(logoutId: string): Observable<UserLogoutResult> {
    return this.http.post<UserLogoutResult>(`${this.endpoint}/sign-out`, logoutId, { withCredentials: true });
  }

    changeExpiredPassowrd(changeExpiredPasswordInfo: ChangeExpiredPasswordModel): Observable<ChangeExpiredPasswordResult> {
        return this.http.post<ChangeExpiredPasswordResult>(`${this.endpoint}/change-expired-password`, changeExpiredPasswordInfo, { withCredentials: true });
    }

  loadCaptcha(): Observable<Blob> {
    return this.http.get(this.captchaEndpoint, {responseType: 'blob', withCredentials: true  });
  }
}
