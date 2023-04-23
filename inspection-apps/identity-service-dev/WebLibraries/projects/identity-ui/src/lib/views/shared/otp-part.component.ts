import { Component, OnInit, Input, EventEmitter, Output, Inject } from '@angular/core';
import { OtpModel } from '../../models/otp-model';
import { AuthenticationServerService } from '../../services/web-api/authentication-server.service';
import { OtpVerificationResultResult } from '../../models/otp-verification-result';
import { OtpPartInput } from '../../models/opt-part-input';
import { Router } from '@angular/router';
import { Location } from '@angular/common';
import { IdentityUiConfig, CONFIG_TOKEN_KEY } from '../../utils/identity-ui-config';

@Component({
  selector: 'idensrv-otp-part',
  templateUrl: './otp-part.component.html',
  styleUrls: ['./otp-part.component.css']
})
export class OtpPartComponent implements OnInit {

  otpInfo: OtpModel;
  verificationResult: OtpVerificationResultResult;
  timeoutTimer: any;
  resendOTPTimer: any;
  remainingSecondsForResend: number;
  remainingSeconds: number;
  isResendFailed: boolean = false;
  errorMsg: string;

  tenantName: string = this._config.getConfigValues().AppTitle;
  tenantLogoUrl: string = this._config.getConfigValues().AppLogo;

  @Input() otpPartInput: OtpPartInput;
  @Output() commandEvent = new EventEmitter();

  constructor(@Inject(CONFIG_TOKEN_KEY) private _config: IdentityUiConfig, private _authService: AuthenticationServerService, private router: Router) { }

  ngOnInit() {

    this.startTimeoutTimer();
    this.startResendTimer();

    this.otpInfo = {
      otpValue: this.otpPartInput.issues
    };
  }

  private startTimeoutTimer() {
    this.remainingSeconds = this.otpPartInput.attemptsTimeoutInMintutes * 60;
    this.timeoutTimer = setInterval(() => {
      this.remainingSeconds--;
      if (this.remainingSeconds <= 0) {
        this.cancelTimeoutTimer();
        // this.router.navigate([''], { replaceUrl: true });
      }
    }, 1000);
  }

  private startResendTimer() {
    this.remainingSecondsForResend = 2 * 60;
    this.resendOTPTimer = setInterval(() => {
      this.remainingSecondsForResend--;
      if (this.remainingSecondsForResend <= 0) {
        this.cancelResendOTPTimer();
      }
    }, 1000);
  }

  private cancelResendOTPTimer() {
    if (this.resendOTPTimer != null) {
      clearInterval(this.resendOTPTimer);
      this.remainingSecondsForResend = 0;
      this.resendOTPTimer = null;
    }
  }

  private cancelTimeoutTimer() {
    if (this.timeoutTimer != null) {
      clearInterval(this.timeoutTimer);
      this.remainingSeconds = 0;
      this.timeoutTimer = null;
    }
  }

  disableSubmit(): boolean {
    return this.remainingSeconds <= 0 ||
      this.verificationResult 
        && (this.verificationResult.isAccountLocked || this.verificationResult.isOtpExpired)
      || !this.timeoutTimer;
  }

  disableResend(): boolean {
    return this.remainingSecondsForResend > 0;
  }

  showTrialTimeout(): boolean {
    if (this.verificationResult) {
      return (this.remainingSeconds <= 0 && !this.verificationResult.isAccountLocked) ||
        this.verificationResult.isOtpExpired;
    } else {
      return this.remainingSeconds <= 0;
    }
  }

  resendOTP() {

    this._authService.resendOtp().subscribe(result => {
      this.cancelTimeoutTimer();
      this.startTimeoutTimer();
    }
      , error => {
        if (error) {
          this.isResendFailed = true;
          this.errorMsg = error.error;
        }
      });

    this.cancelResendOTPTimer();
    this.startResendTimer();
  }

  onSubmit(form) {
    if (form.invalid) {
      return;
    }

    this._authService.verifyOtp(this.otpInfo).subscribe(result => {
      if (result.success) {
        this.commandEvent.emit({ 'command': 'otpSuccess' });
      } else if (result.isOtpExpired || result.isAccountLocked || result.isPasswordExpired) {
        this.cancelTimeoutTimer();
        if (result.isPasswordExpired) {
          this.commandEvent.emit({
            'command': 'expiredPasswordDetected',
            changeExpiredPasswordToken: result.changeExpiredPasswordToken
          });
        }
      }
      this.verificationResult = result;
    });
  }
}
