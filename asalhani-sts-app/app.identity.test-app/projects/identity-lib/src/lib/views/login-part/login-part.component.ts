import {Component, EventEmitter, Inject, Input, OnInit, Output} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {CONFIG_TOKEN_KEY, IdentityUiConfig} from "../../utils/identity-ui-config";
import {AuthenticationServerService} from "../../services/authentication-server-service";
import {UserLoginModel} from "../../models/user-login-model";
import {UserLoginResult} from "../../models/user-login-result";

@Component({
  selector: 'lib-login-part',
  templateUrl: './login-part.component.html',
  styleUrls: ['./login-part.component.css']
})
export class LoginPartComponent implements OnInit {

  @Input() returnUrl: string;
  @Output() commandEvent = new EventEmitter();

  loginForm: FormGroup;

  loginResult: UserLoginResult;
  constructor(@Inject(CONFIG_TOKEN_KEY) private _config: IdentityUiConfig
              , private formBuilder: FormBuilder
              , private _authService: AuthenticationServerService) {

  }

  ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      idNumber: ['', Validators.compose([Validators.required/*, AwqafValidators.ninOrIqama(IdType.NIN_OR_IQAMA)*/])],
    });
  }

  private initLogingForm() {

  }

  get f() {
    return this.loginForm?.controls;
  }

  onLogin(formIsValid: boolean) {
    if (formIsValid) {
      let loginData: UserLoginModel = new class implements UserLoginModel {
        loginName: string;
      };
      loginData.loginName = this.loginForm.controls['idNumber'].value;
      this._authService.authenticate(loginData).subscribe(result => {
        if (result.success) {
          window.location.href = this.returnUrl; // result.redirectUrl;
        } else if (result.isOtpRequired) {
          this.commandEvent.emit({ 'command': 'loginName', 'loginName': loginData.loginName });
          this.commandEvent.emit(Object.assign({ 'command': 'otp' }, result));
        } else if (result.isPasswordExpired) {
          this.commandEvent.emit({ 'command': 'loginName', 'loginName': loginData.loginName });
          this.commandEvent.emit({
            'command': 'expiredPasswordDetected',
            changeExpiredPasswordToken: result.changeExpiredPasswordToken
          });
        }
        this.loginResult = result;
      }, error => {
        console.error('Fetching data from the server failed. Please try again.');
      });
    }
  }
}
