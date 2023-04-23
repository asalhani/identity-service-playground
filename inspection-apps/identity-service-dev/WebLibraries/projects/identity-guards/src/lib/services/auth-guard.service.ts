import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuardService implements CanActivate {

  constructor(private authService: AuthService) { }

  canActivate(): Promise<boolean> {
    return new Promise((resolve, reject) => {

      this.authService.isLoggedIn().then(result => {
        if (!result)
          this.authService.startAuthentication();
        resolve(result);
      });
    });
  }
}
