import { Component, OnInit, Inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { OtpPartInput } from '../../models/opt-part-input';
import { AuthenticationServerService } from '../../services/web-api/authentication-server.service';
import { ChangePasswordMode } from '../../models/change-password-modes';
import { CONFIG_TOKEN_KEY, IdentityUiConfig } from '../../utils/identity-ui-config';

@Component({
  selector: 'idensrv-reset-password-view',
  templateUrl: './reset-password-view.component.html',
  styleUrls: ['./reset-password-view.component.css']
})
export class ResetPasswordViewComponent implements OnInit {

  public changePasswordMode = ChangePasswordMode;
  isOtpVisible = false;
  token: string;
  isTokenValid = true;
  otpInput: OtpPartInput;
  resetSuccess = false;

  tenantName: string = this._config.getConfigValues().AppTitle;
  tenantLogoUrl: string = this._config.getConfigValues().AppLogo;

  constructor(@Inject(CONFIG_TOKEN_KEY) private _config: IdentityUiConfig, private _route: ActivatedRoute,
    private _authService: AuthenticationServerService) { }

  ngOnInit() {

    // grap token value from URL
    this.token = this._route.snapshot.params['token'];
    if (this.token == null || this.token.length !== 36) {
      this.isTokenValid = false;
    } else {
      // validate token on server
      this._authService.validateEmailToken(this.token).subscribe(res => {
        if (res) {
          this.isTokenValid = true;
        } else {
          this.isTokenValid = false;
        }
      });
    }
  }

  onCommandEvent(any) {
    if (any.command === 'otpSuccess') {
      this.isOtpVisible = false;
      this.resetSuccess = true;
    } else if (any.command === 'otpRequired') {
      this.otpInput = <OtpPartInput>any;
      this.isOtpVisible = true;
    }
  }

}
