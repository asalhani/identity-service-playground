import { Component, OnInit, Inject, EventEmitter, Output, Input, OnChanges } from '@angular/core';
import { IdentityUiConfig, CONFIG_TOKEN_KEY } from '../../utils/identity-ui-config';
import { ChangePasswordMode } from '../../models/change-password-modes';
import { ResetPasswordModel } from '../../models/reset-password-model';
import { ChangeExpiredPasswordModel } from '../../models/change-expired-password-model';
import { AuthenticationServerService } from '../../services/web-api/authentication-server.service';
import { TranslateService } from '@ngx-translate/core';
import { Router } from '@angular/router';

@Component({
  selector: 'idensrv-change-password-part',
  templateUrl: './change-password-part.component.html',
  styleUrls: ['./change-password-part.component.css']
})

export class ChangePasswordPartComponent implements OnInit {

  public changePasswordMode = ChangePasswordMode;
  captchaString: string;
  newPassword: string;
  confirmedPasswordValue: string;
  isProcessSuccess = false;
  tokenExpired = false;
  captchaSiteKey = this._config.getConfigValues().recaptchaSiteKey;
  captchaType: string = this._config.getConfigValues().captchaType;
  tenantName: string = this._config.getConfigValues().AppTitle;
  tenantLogoUrl: string = this._config.getConfigValues().AppLogo;
  isSubmitted = false;
  error: string;
  changeExpiredPasswordInfo: ChangeExpiredPasswordModel;
  resetPasswordInfo: ResetPasswordModel;
  imageToShow: any;
  showInvalidCaptchaMsg:boolean=false;

  readonly changePasswordError: string = this.translate.instant('Your password is expired');

  @Input() mode: ChangePasswordMode;
  @Input() token: string;
  @Input() loginName: string;
  @Output() commandEvent = new EventEmitter();

  constructor(
    @Inject(CONFIG_TOKEN_KEY) private _config: IdentityUiConfig,
    private _authService: AuthenticationServerService,
    private translate: TranslateService) { }
    loginURL:string;

  ngOnInit() {    
    this.changeExpiredPasswordInfo = {
      newPassword: '',
      changePasswordToken: this.token,
      loginName: this.loginName,
      captchaString: '',
      isInvalidCaptcha:false
    };
    this.resetPasswordInfo = {
      password: '',
      emailToken: this.token,
      captchaString: '',
    };
    switch (this.mode) {
      case ChangePasswordMode.ResetPassword: {
        break;
      }
      case ChangePasswordMode.ChangeExpiredPassword: {
        this.error = this.changePasswordError;
        break;
      }
    }
    this.loadCaptcha();
  }

  loadCaptcha()
  {
       this._authService.loadCaptcha().subscribe(data =>
      {
         let reader = new FileReader();
         reader.addEventListener("load", () => {
         this.imageToShow = reader.result;
       }, false);
       if (data)
       {
            reader.readAsDataURL(data);
       }
      }, error => console.log(error));
  }

  captchaResolved(event) {
    this.captchaString = event;
  }

  

  nextClicked(form) {
    if (form.invalid || !this.captchaString) { return; }

    this.error = '';
    this.isSubmitted = true;

    switch (this.mode) {
      case ChangePasswordMode.ResetPassword: {
        this.resetPasswordInfo.password = this.newPassword;
        this.resetPasswordInfo.captchaString = this.captchaString;
        this.resetPasswordInfo.emailToken = this.token;
        this.showInvalidCaptchaMsg=false;
        this._authService.resetPassword(this.resetPasswordInfo).subscribe(result => {
          if (result.success) {
            this.commandEvent.emit(Object.assign({ command: 'otpRequired' }, result));
          } else {
            if(result.isInvalidCaptcha)
            { this.showInvalidCaptchaMsg =true;
              this.loadCaptcha();
            }
            this.error = result.issues;
            this.isSubmitted = false;
          }
        });
        break;
      }
      case ChangePasswordMode.ChangeExpiredPassword: {
        this.changeExpiredPasswordInfo.newPassword = this.newPassword;
        this.changeExpiredPasswordInfo.captchaString = this.captchaString;
        this.showInvalidCaptchaMsg=false;
        this._authService.changeExpiredPassowrd(this.changeExpiredPasswordInfo).subscribe(result => {
          if (result.success) {
            this.isProcessSuccess = true;
          }
          else {
            
            if(result.isInvalidCaptcha)
            {
              this.showInvalidCaptchaMsg =true;
              this.isSubmitted = false;
              this.loadCaptcha();
            }
            else if (result.isInvalidNewPassword) {
              this.error = result.invalidPasswordIssues;
              this.isSubmitted = false;
              this.loadCaptcha();
            }
            else if (result.isInvalidRequest) {
              this.error = '';
              this.tokenExpired = true;
            }
          }
        });
        break;
      }
    }
  }

  onCommandEvent(any) {
    if (any.command === 'expiredPasswordDetected') {
      this.changeExpiredPasswordInfo = any.changeExpiredPasswordToken;
    } else if (any.command === 'loginName') {
      this.changeExpiredPasswordInfo = any.loginName;
    }
  }
}
