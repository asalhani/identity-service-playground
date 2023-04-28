import { Component, OnInit } from '@angular/core';
import {AuthService, LoginTypeEnum} from "identty-lib";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styles: [
  ]
})
export class HomeComponent implements OnInit {

  constructor(private _authService: AuthService) { }

  ngOnInit(): void {
  }

  onEmployeeLogin() {
    this._authService.startAuthenticationCustom(LoginTypeEnum.employee);
  }
}
