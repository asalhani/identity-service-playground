import { Injectable } from '@angular/core';
import { Response } from '@angular/http';
import { TranslateLoader } from '@ngx-translate/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IdentityUiConfig, CONFIG_TOKEN_KEY } from './identity-ui-config';
import { LanguageConfig } from './language-config';

@Injectable({
  providedIn: 'root',
  deps: [{ provide: CONFIG_TOKEN_KEY, useValue: IdentityUiConfig }]
})
export class CustomTranslationLoader implements TranslateLoader {

  constructor(private http: HttpClient, private config: LanguageConfig) { }

  getTranslation(lang: string): Observable<any> {
    const apiUrl = this.config.getTranslationUrl(lang);

    return Observable.create(observer => {
      this.http.get(apiUrl).subscribe((res: Response) => {
        observer.next(res);
        observer.complete();
      },
        _ => {
          // If retrieving from API failed, then switch to local ..
          this.http.get(`/assets/i18n/${lang}.json`).subscribe((res: Response) => {
            observer.next(res);
            observer.complete();
          });
        }
      );
    });
  }
}
