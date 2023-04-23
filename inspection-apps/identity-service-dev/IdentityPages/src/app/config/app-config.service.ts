import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { AppConfig } from './app-config.model';
import { APP_BASE_HREF } from '@angular/common';

@Injectable({
  providedIn: 'root',

})
export class AppConfigService {

  static settings: AppConfig = {};

  // headers = new HttpHeaders().set(InterceptorSkipHeader, '');

  constructor(private http: HttpClient, @Inject(APP_BASE_HREF) private baseHref: string) { }

  load(): Promise<any> {
    const jsonFile = this.baseHref + `/assets/config/app.config.${environment.name}.json`;

    return new Promise<void>((resolve, reject) => {
      this.http.get(jsonFile/*, { headers: this.headers }*/).toPromise().then((data) => {
        AppConfigService.settings = data as AppConfig;
        resolve();
      }).catch((_error: any) => {
        console.log(`Could not load file '${jsonFile}': ${JSON.stringify(_error)}`);
        reject();
      });
    });
  }
}
