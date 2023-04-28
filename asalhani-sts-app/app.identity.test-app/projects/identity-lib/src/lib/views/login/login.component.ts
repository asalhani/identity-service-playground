import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {LoginTypeEnum} from "../../models/login-type-enum";

@Component({
  selector: 'lib-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  returnUrl: string;
  loginName: string;
  activatedLoginType: string | null | undefined;

  // to use enum in html template
  LoginTypeEnum = LoginTypeEnum;

  constructor(private _route: ActivatedRoute) {
    this.activatedLoginType = _route.snapshot.data['loginType'];

    this._route.queryParams.subscribe(params => {
      debugger
      this.returnUrl = params['returnUrl'];
      let decodedUrl = decodeURIComponent(this.returnUrl);

      this.activatedLoginType = this.getQueryVariable(decodedUrl, 'loginType')?.toLowerCase();


    });

    // let test = this.getQueryParamFromMalformedURL('loginType');
  }

  ngOnInit(): void {
    if (!this.returnUrl || this.returnUrl.length === 0) {
      console.error('Cannot find the \'returnUrl\' parameter. This page shouldn\'t be accessed directly.');
    }
  }

  getQueryVariable(url: string, querystringName: string): string | null {
    var query = url;
    var vars = query.split('&');
    for (var i = 0; i < vars.length; i++) {
      var pair = vars[i].split('=');
      if (decodeURIComponent(pair[0]) == querystringName) {
        return decodeURIComponent(pair[1]);
      }
    }
    return null;
    console.log('Query variable %s not found', querystringName);
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
