import {Injectable} from '@angular/core';
import {HttpErrorResponse, HttpEvent, HttpHandler, HttpHeaders, HttpInterceptor, HttpRequest} from '@angular/common/http';
import {from, Observable} from 'rxjs';
import {AuthService} from './auth.service';
import {Constants} from './constants';
import {catchError} from 'rxjs/operators';
import {Router} from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthInterceptorService implements HttpInterceptor {
  constructor(private _authService: AuthService, private _router: Router) {
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (req.url.startsWith(Constants.apiRoot)) {
      return from(
        this._authService.getAccessToken()
          .then(token => {
            const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
            const authRequest = req.clone({headers});
            return next.handle(authRequest)
              .pipe(
                catchError((err: HttpErrorResponse) => {
                  if (err && (err.status === 401 || err.status === 403)) {
                    this._router.navigate(['/unauthorized']);
                  }
                  throw new Error('error in a request ' + err.status);
                })
              ).toPromise();
          })
      );
    } else {
      return next.handle(req);
    }
  }
}
