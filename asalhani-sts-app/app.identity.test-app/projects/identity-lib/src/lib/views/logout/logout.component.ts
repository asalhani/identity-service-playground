import { Component, OnInit } from '@angular/core';
import {AuthenticationServerService} from "../../services/authentication-server-service";
import {ActivatedRoute} from "@angular/router";

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styles: [
  ]
})
export class LogoutComponent implements OnInit {

  step = 0;
  logoutId: string;
  clientName: string;

  constructor(private _authService: AuthenticationServerService, private _route: ActivatedRoute) { }

  ngOnInit() {
    this.logoutId = this._route.snapshot.queryParams['logoutId'];
    if (this.logoutId == null) {
      this.logoutId = '';
    }

    this._authService.signOutAttempt(this.logoutId).subscribe(result => {
      if (result.isShowPrompt) {
        this.clientName = result.clientName;
        this.step = 1;
      } else {
        this.completeSignOut(result.postLogoutRedirectUri);
      }
    });
  }

  signOutAssured() {
    this._authService.signOutAssured(this.logoutId).subscribe(result => {
      this.clientName = result.clientName;
      this.completeSignOut(result.postLogoutRedirectUri);
    });
  }

  private completeSignOut(redirectUrl: string) {
    if (redirectUrl) {
      window.location.href = redirectUrl;
      // this._router.navigateByUrl(redirectUrl);
    } else {
      this.step = 2;
    }
  }

}
