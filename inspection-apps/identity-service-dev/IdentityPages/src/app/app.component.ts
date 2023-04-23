import { Component } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { TitleService } from './shared/title/title.service';
import { AppConfigService } from './config/app-config.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  constructor(translate: TranslateService, private titleService: TitleService) {
    translate.setDefaultLang('ar');
    translate.use('ar');
  }

  ngOnInit() {
    this.titleService.boot();
  }
}
