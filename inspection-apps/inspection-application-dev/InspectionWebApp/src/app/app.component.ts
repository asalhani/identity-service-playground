import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Formio } from 'elm-formiojs';
import { AppConfigService } from './config/app-config.service';
import { TitleService } from 'elm-common-ui-helpers-library';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styles: ['']
})
export class AppComponent implements OnInit {
  serviceUrl: any;

  constructor(private titleService: TitleService, private translate: TranslateService) {
    this.translate.addLangs(['ar', 'en']);
    this.translate.setDefaultLang('ar');
    this.translate.use('ar');
  }

  ngOnInit() {
    this.serviceUrl = AppConfigService.settings.ServiceURL;
    this.formIoSetup();
    this.titleService.boot();
  }

  private formIoSetup(): void {
    const _comp = this;

    const serviceUrlPlugin = {
      priority: 0,
      preRequest: function (requestArgs) {
        requestArgs.url = `${_comp.serviceUrl}${requestArgs.url}`;
      },

      preStaticRequest: function (requestArgs) {
        requestArgs.url = `${_comp.serviceUrl}${requestArgs.url}`;
      },
    };

    Formio.registerPlugin(serviceUrlPlugin, 'serviceUrl');
  }
}
