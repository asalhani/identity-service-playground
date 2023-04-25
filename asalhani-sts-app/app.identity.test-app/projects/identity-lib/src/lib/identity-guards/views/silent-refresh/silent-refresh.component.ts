import { Component, OnInit } from '@angular/core';
import {AuthService} from "../../services/auth.service";

@Component({
  selector: 'app-silent-refresh',
  template: `
    <p>
      silent-refresh works!
    </p>
  `,
  styles: [
  ]
})
export class SilentRefreshComponent implements OnInit {

  constructor(private _authService: AuthService) { }

  ngOnInit() {
    console.log('SilentRefreshComponent: ngOnInit');
    this._authService.completeSilentRefresh();
  }

}

