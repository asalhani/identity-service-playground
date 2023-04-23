import { Component, OnInit, Inject, EventEmitter, Output, Input } from '@angular/core';
import { UserLoginModel } from '../../models/user-login-model';
import { UserLoginResult } from '../../models/user-login-result';
import { IdentityUiConfig, CONFIG_TOKEN_KEY } from '../../utils/identity-ui-config';
import { AuthenticationServerService } from '../../services/web-api/authentication-server.service';
import { CaptchaInput } from '../../models/captcha-input';


@Component({
  selector: 'idensrv-login-part',
  templateUrl: './login-part.component.html',
  styleUrls: ['./login-part.component.css']
})

export class LoginPartComponent implements OnInit {

  @Input() returnUrl: string;
  @Output() commandEvent = new EventEmitter();

  loading = false;
  loginFailure = false;
  loginData: UserLoginModel;
  loginResult: UserLoginResult;
  captchaSiteKey = this._config.getConfigValues().recaptchaSiteKey;
  captchaType: string = this._config.getConfigValues().captchaType;
  tenantName: string = this._config.getConfigValues().AppTitle;
  tenantLogoUrl: string = this._config.getConfigValues().AppLogo;
  captchaInput: CaptchaInput = new CaptchaInput; 
  imageToShow: any;

  constructor(@Inject(CONFIG_TOKEN_KEY) private _config: IdentityUiConfig, private _authService: AuthenticationServerService)
 { }

  ngOnInit() {
    this.loginData = {
      loginName: null,
      password: null,
      captchaString: null
    };
    this.loadCaptcha();   
  }

  loadCaptcha()
  {
       this._authService.loadCaptcha().subscribe(data => 
      {
         let reader = new FileReader();
         reader.addEventListener("load", () => {
         this.imageToShow = reader.result;
         this.loginData.captchaString = '';
       }, false);
       if (data) 
       {
            reader.readAsDataURL(data);
       }        
      }, error => console.log(error));
  } 
  

  login(formIsValid) {
    if (formIsValid) {
      this._authService.authenticate(this.loginData).subscribe(result => {
        if (result.success) {
          window.location.href = this.returnUrl; // result.redirectUrl;
        } else if (result.isOtpRequired) {
          this.commandEvent.emit({ 'command': 'loginName', 'loginName': this.loginData.loginName });
          this.commandEvent.emit(Object.assign({ 'command': 'otp' }, result));
        } else if (result.isPasswordExpired) {
          this.commandEvent.emit({ 'command': 'loginName', 'loginName': this.loginData.loginName });
          this.commandEvent.emit({
            'command': 'expiredPasswordDetected',
            changeExpiredPasswordToken: result.changeExpiredPasswordToken
          });
        }
        this.loadCaptcha();
        this.loginResult = result;
      }, error => {
        console.error('Fetching data from the server failed. Please try again.');
      });
    }
  }

  captchaResolved(event) {
    this.loginData.captchaString = event;
  }

}

