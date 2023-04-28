import {Component, EventEmitter, Input, OnInit, Output} from "@angular/core";
import {FormGroup} from "@angular/forms";
import {UserLoginResult} from "../models/user-login-result";
import {EmployeeUserLoginModel, PublicUserLoginModel} from "../models/user-login-model";

@Component({
  selector: 'lib-login-base',
  template: `<p> base works! </p>`,
  styles: [  ]
})
export class LoginBaseView implements OnInit{
  @Input() returnUrl: string;
  @Output() commandEvent = new EventEmitter();
  @Input() loginType: string;

  loginForm: FormGroup;
  loginResult: UserLoginResult;
  loginData: PublicUserLoginModel | EmployeeUserLoginModel;

  get f() {
    return this.loginForm?.controls;
  }

  ngOnInit(): void {
  }

  handleLoginResponse(loginResult: UserLoginResult){
    if (loginResult.success) {
      window.location.href = this.returnUrl; // result.redirectUrl;
    } else if (loginResult.isPasswordExpired) {
      this.commandEvent.emit({ 'command': 'loginName', 'loginName': this.loginData.loginName });
      this.commandEvent.emit({
        'command': 'expiredPasswordDetected',
        changeExpiredPasswordToken: loginResult.changeExpiredPasswordToken
      });
    }
    this.loginResult = loginResult;
  }

}
