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

    // try to pick the value from routing data
    this.activatedLoginType = _route.snapshot.data['loginType'];

    // if no value provided, then try to pick value from returnUrl querystring (after decode it)
      this._route.queryParams.subscribe(params => {
        this.returnUrl = params['returnUrl'];
        if(!this.activatedLoginType){
          let decodedUrl = decodeURIComponent(this.returnUrl);
          this.activatedLoginType = this.getQueryVariable(decodedUrl, 'loginType')?.toLowerCase();
          if (!this.activatedLoginType)
            this.activatedLoginType = LoginTypeEnum.public.toLowerCase();
        }
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

  getQueryVariable(url: string, querystringName: string): string | null {
    const query = url;
    const vars = query.split('&');
    for (let i = 0; i < vars.length; i++) {
      const pair = vars[i].split('=');
      if (decodeURIComponent(pair[0]) == querystringName) {
        return decodeURIComponent(pair[1]);
      }
    }
    return null;
  }
}
