import { HttpClient, HttpHeaders } from '@angular/common/http';
import { TranslateLoader } from '@ngx-translate/core';
import { Observable } from 'rxjs';
import { InterceptorSkipHeader } from '../config/constants';
import { APP_BASE_HREF } from '@angular/common';
import { Inject } from '@angular/core';

export class AppTranslateHttpLoader implements TranslateLoader {
  constructor(
    private http: HttpClient, public prefix: string , public suffix: string ) { }

  /**
   * Gets the translations from the server while skipping interceptor ..
   */
  public getTranslation(lang: string): Observable<Object> {
    const headers = new HttpHeaders().set(InterceptorSkipHeader, '');
    return this.http.get(`${this.prefix}${lang}${this.suffix}`, { headers: headers });
  }
}
