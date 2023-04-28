import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {LoginBaseView} from "../login-base-view";

@Component({
  selector: 'lib-ad-login-part',
  templateUrl: './ad-login-part.component.html',
  styleUrls: ['./ad-login-part.component.css']
})
export class AdLoginPartComponent extends  LoginBaseView implements OnInit {

  constructor() {
    super();
  }

  override ngOnInit(): void {

  }

}
