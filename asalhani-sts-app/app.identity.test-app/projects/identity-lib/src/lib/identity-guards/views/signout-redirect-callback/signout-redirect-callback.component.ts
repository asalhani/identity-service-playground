import { Component, OnInit } from '@angular/core';
import {AuthService} from "../../services/auth.service";
import {Router} from "@angular/router";

@Component({
  selector: 'lib-signout-redirect-callback',
  template: `
    <p>
      signout-redirect-callback works!
    </p>
  `,
  styles: [
  ]
})
export class SignoutRedirectCallbackComponent implements OnInit {

  constructor(private _authService: AuthService, private _router: Router) { }

  ngOnInit(): void {
    this._authService.finishLogout()
      .then(_ => {
        this._router.navigate(['/'], { replaceUrl: true });
      });
  }
}
