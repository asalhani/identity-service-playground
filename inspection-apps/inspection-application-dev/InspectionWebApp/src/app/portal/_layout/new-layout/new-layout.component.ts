import { Component, OnInit } from '@angular/core';
import { AppTitle, TitleService } from 'elm-common-ui-helpers-library';

@Component({
  selector: 'app-new-layout',
  templateUrl: './new-layout.component.html',
  styleUrls: ['./new-layout.component.scss']
})
export class NewLayoutComponent implements OnInit {

  appTitle: AppTitle = new AppTitle();

  constructor(private title: TitleService) {
    this.title.getTitles().subscribe((titles) => {
      this.appTitle.title = titles.title;
      this.appTitle.breadcrumbTitle = titles.breadcrumbTitle;
      this.appTitle.breadcrumbTitleLvl2 = titles.breadcrumbTitleLvl2;
    });
  }

  ngOnInit() {
  }
}
