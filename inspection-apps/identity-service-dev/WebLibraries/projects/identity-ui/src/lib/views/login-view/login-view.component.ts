import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { OtpPartInput } from '../../models/opt-part-input';
import { ChangePasswordMode } from '../../models/change-password-modes';

@Component({
  selector: 'idensrv-login-view',
  templateUrl: './login-view.component.html',
  styleUrls: ['./login-view.component.css']
})

export class LoginViewComponent implements OnInit {

  public changePasswordMode = ChangePasswordMode;
  isOtpVisible = false;
  isChangePasswordVisible = false;
  returnUrl: string;
  otpInput: OtpPartInput;
  token: string;
  loginName: string;

  constructor(private _route: ActivatedRoute) {
    this._route.queryParams.subscribe(params => {
      this.returnUrl = params['returnUrl'];
    });
  }

  ngOnInit() {
    if (!this.returnUrl || this.returnUrl.length === 0) {
      console.error('Cannot find the \'returnUrl\' parameter. This page shouldn\'t be accessed directly.');
    }
  }

  onCommandEvent(any) {
    if (any.command === 'otp') {
      this.isOtpVisible = true;
      this.otpInput = <OtpPartInput>any;
    } else if (any.command === 'loginName') {
      this.loginName = any.loginName;
    } else if (any.command === 'expiredPasswordDetected') {
      // show change password prompt with expired password message
      this.isOtpVisible = false;
      this.isChangePasswordVisible = true;
      // capture change expired password token
      this.token = any.changeExpiredPasswordToken;
    } else if (any.command === 'otpSuccess') {
      window.location.href = this.returnUrl;
    }
  }
}
