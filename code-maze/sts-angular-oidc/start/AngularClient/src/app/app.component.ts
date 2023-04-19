import {Component, OnInit} from '@angular/core';
import {AuthService} from './shared/services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'CompanyEmployees.Client.Oidc';

  /*
  We need to realize that after the login action on the IDP side, the redirection happens back to the Angular
  application causing the Angular app to refresh itself (a fresh load of the application). So, let’s modify
  the app.component file to check if the user is authenticated:
   */
  public userAuthenticated = false;

  constructor(private _authService: AuthService){
    this._authService.loginChanged
      .subscribe(userAuthenticated => {
        this.userAuthenticated = userAuthenticated;
      });
  }

  ngOnInit(): void {
    this._authService.isAuthenticated()
      .then(userAuthenticated => {
        this.userAuthenticated = userAuthenticated;
      });
  }
}
