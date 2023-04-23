import { Component, OnInit, Inject } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { IdentityGuardsConfig } from '../../models/identity-guards-config';

@Component({
  selector: 'app-auth-callback',
  templateUrl: './auth-callback.component.html',
  styleUrls: ['./auth-callback.component.css']
})
export class AuthCallbackComponent implements OnInit {

  showRedirectMessage: boolean = false;
  postLoginRedirectUrl: string = '/';

  constructor(
    @Inject('identity_guards_config') private _config: IdentityGuardsConfig,
    private _authService: AuthService,
    private _router: Router
  ) {
    let url = _config.getConfigValues().postLoginRedirectUrl;
    if (url)
      this.postLoginRedirectUrl = url;
    else
      console.warn(`'Couldn't find a value for 'postLoginRedirectUrl' in 'IdentityGuardsSettings'. Using '/' instead.`);
  }

  ngOnInit() {
    this._authService.completeAuthentication().then(() => {
      setTimeout(x => {
        this._router.navigateByUrl(this.postLoginRedirectUrl);
      }, 100);

      setTimeout(x => {
        this.showRedirectMessage = true;
      }, 1000);
    });
  }
}
