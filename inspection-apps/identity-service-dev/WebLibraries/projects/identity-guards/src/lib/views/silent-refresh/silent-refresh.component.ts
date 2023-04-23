import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'idengrd-silent-refresh',
  templateUrl: './silent-refresh.component.html',
  styleUrls: ['./silent-refresh.component.css']
})
export class SilentRefreshComponent implements OnInit {

  constructor(private _authService: AuthService) { }

  ngOnInit() {
    console.log('SilentRefreshComponent: ngOnInit');
    this._authService.completeSilentRefresh();
  }

}
