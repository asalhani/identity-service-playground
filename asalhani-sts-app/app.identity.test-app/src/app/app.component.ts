import { Component } from '@angular/core';
import {AuthService, LoginTypeEnum} from "identty-lib";
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  title = 'identity-app';
  isLoggedIn: boolean = false;

  constructor(private _authService: AuthService, private _http: HttpClient) {
    this._authService.getIsLoggedIn().subscribe(isLoggedIn =>{
      this.isLoggedIn = isLoggedIn;
    });
  }

  signout() {
    this._authService.signOut();
  }

  callSecureAPI() {
    this._http.get<string>('http://localhost:22213/api/values', {
      headers: {
        Authorization: this._authService.getAuthorizationHeaderValue()
      }
    }).subscribe(result => console.log(`Calling Secure Method result: ${JSON.stringify(result)}`));
  }

  onEmployeeLogin() {
    this._authService.startAuthenticationCustom(LoginTypeEnum.employee);
  }
}
