import { Component, OnInit, Inject } from '@angular/core';
import { ForgotPasswordModel } from '../../models/forgot-password-model';
import { AuthenticationServerService } from '../../services/web-api/authentication-server.service';
import { IdentityUiConfig, CONFIG_TOKEN_KEY } from '../../utils/identity-ui-config';
import { CaptchaInput } from '../../models/captcha-input';

@Component({
  selector: 'idensrv-forgot-password-view',
  templateUrl: './forgot-password-view.component.html',
  styleUrls: ['./forgot-password-view.component.css']
})

export class ForgotPasswordViewComponent implements OnInit {

  forgotPasswordModel: ForgotPasswordModel;
  captchaSiteKey = this._config.getConfigValues().recaptchaSiteKey;
  captchaType: string = this._config.getConfigValues().captchaType;
  tenantName: string = this._config.getConfigValues().AppTitle;
  tenantLogoUrl: string = this._config.getConfigValues().AppLogo;
  captchaInput: CaptchaInput = new CaptchaInput; 
  resetPasswordUrl: string; // only for testing purposes.
  showSuccess = false;
  imageToShow: any;

  constructor(@Inject(CONFIG_TOKEN_KEY) private _config: IdentityUiConfig, private _authService: AuthenticationServerService) {
    this.forgotPasswordModel = {
      captchaString: '',
      email: '',
      emailToken: '',
      isInvalidCaptcha:false
    };
  }

  ngOnInit() {
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

  onSubmit(form) {
    if (form.invalid || !this.forgotPasswordModel.captchaString) {
      return;
    }

    this._authService.forgotPassword(this.forgotPasswordModel).subscribe(result => {
     
      if(result!=null && result!= undefined && !result.success  && this.captchaType!="Google" && result.isInvalidCaptcha)
      {
        this.loadCaptcha();
        this.forgotPasswordModel.isInvalidCaptcha=true;
        return;       
      }

      this.resetPasswordUrl = result.info; // this field will hold the reset password url only in debug mode.
      this.showSuccess = true;
    },error => {console.log(error);this.loadCaptcha();});
  }

  captchaResolved(event) {
    this.forgotPasswordModel.captchaString = event;
  }

}
