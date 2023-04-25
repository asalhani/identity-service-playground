import { Component } from '@angular/core';
import {AuthService} from "identty-lib";
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  title = 'identity-app';

  constructor(private _authService: AuthService, private _http: HttpClient) {

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

}
