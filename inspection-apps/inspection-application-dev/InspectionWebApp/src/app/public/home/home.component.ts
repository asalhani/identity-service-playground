import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { DEFAULT_LANG } from '../../config/constants';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styles: ['']
})
export class HomeComponent implements OnInit {

  constructor(translate: TranslateService) {
    translate.setDefaultLang(DEFAULT_LANG);
    translate.use(DEFAULT_LANG);
  }

  ngOnInit() { }

}
