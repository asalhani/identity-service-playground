import { Injectable } from '@angular/core';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { filter, map, mergeMap, flatMap } from 'rxjs/operators';
import { Observable, BehaviorSubject } from 'rxjs';
import { TranslateService } from '@ngx-translate/core';
import { AppTitle } from './app-title';
import { AppConfigService } from 'src/app/config/app-config.service';

@Injectable({
  providedIn: 'root'
})
export class TitleService {
  default_title = 'بوابة تفتيش';
  subject: BehaviorSubject<AppTitle> = new BehaviorSubject<AppTitle>(
    new AppTitle()
  );

  constructor(
    private router: Router,
    private activeRoute: ActivatedRoute,
    private title: Title,
    private translate: TranslateService,
  ) { }

  boot() {
    this.default_title = AppConfigService.settings.AppTitle || this.default_title;

    this.getTitles().subscribe((titles) => {
      const tabTitle = titles.breadcrumbTitleLvl2 || titles.breadcrumbTitle || titles.title;
      this.title.setTitle(tabTitle ? `${tabTitle} • ${this.default_title}` : this.default_title);
    });
  }

  setTitles(title: AppTitle) {
    this.subject.next(title);
  }

  getTitles(): Observable<AppTitle> {
    return this.router.events.pipe(
      filter((event) => event instanceof NavigationEnd),
      map(() => this.activeRoute),
      map((route) => {
        while (route.firstChild) { route = route.firstChild; }
        return route;
      }),
      filter((route) => route.outlet === 'primary'),
      mergeMap((route) => route.data),
      map((data) => {
        let CurrentappTitle = new AppTitle();
        if (data) {
          CurrentappTitle.title = data.title ? this.translate.instant(data.title) : ''
          CurrentappTitle.breadcrumbTitle = data.breadcrumbTitle ? this.translate.instant(data.breadcrumbTitle) : '';
          CurrentappTitle.breadcrumbTitleLvl2 = data.breadcrumbTitleLvl2 ? this.translate.instant(data.breadcrumbTitleLvl2) : '';
          this.subject.next(CurrentappTitle);
        }
        return this.subject.value;
      }),
      flatMap((data) => {
        return this.subject.asObservable();
      })
    );
  }
}

