import {Component, Inject, OnInit} from '@angular/core';
import {IdentityGuardsConfig} from "../../models/identity-guards-config";
import {AuthService} from "../../services/auth.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-auth-callback',
  template: `
    <br>
    <br>
    <br>
    <br>
    <br>
    <br>
    <br>
    <br>
    <div *ngIf="showRedirectMessage">
      Please wait, you're being redirected. <a routerLink="{{postLoginRedirectUrl}}">Home</a>
    </div>

  `,
  styles: [
  ]
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
