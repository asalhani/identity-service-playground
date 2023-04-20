import { Component, OnInit } from '@angular/core';
import {AuthService} from '../shared/services/auth.service';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {

  public isUserAuthenticated: boolean = false;
  private isUserAdmin: boolean;

  constructor(private _authService: AuthService) { }

  ngOnInit(): void {
    this._authService.loginChanged
      .subscribe(res => {
        this.isUserAuthenticated = res;
        this.isAdmin();
      });
  }

  public login = () => {
    this._authService.login();
  }

  public logout = () => {
    this._authService.logout();
  }

  public isAdmin = () => {
    return this._authService.checkIfUserIsAdmin()
      .then(res => {
        this.isUserAdmin = res;
      });
  }

}
