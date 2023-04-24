import { Component, OnInit } from '@angular/core';
import {ActivatedRoute} from "@angular/router";

@Component({
  selector: 'lib-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  returnUrl: string;
  loginName: string;

  constructor(private _route: ActivatedRoute) {
    this._route.queryParams.subscribe(params => {
      this.returnUrl = params['returnUrl'];
    });
  }

  ngOnInit(): void {
    if (!this.returnUrl || this.returnUrl.length === 0) {
      console.error('Cannot find the \'returnUrl\' parameter. This page shouldn\'t be accessed directly.');
    }
  }

  onCommandEvent({any}: { any: any }) {
     if (any.command === 'loginName') {
      this.loginName = any.loginName;
       // window.location.href = this.returnUrl;
    } else if (any.command === 'otpSuccess') {
      window.location.href = this.returnUrl;
    }
  }

}
