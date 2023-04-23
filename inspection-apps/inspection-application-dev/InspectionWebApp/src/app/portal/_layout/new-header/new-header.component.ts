import { Component, ElementRef, Inject, OnInit } from '@angular/core';
import { AuthService } from 'identity-guards';
import { HttpClient } from '@angular/common/http';
import { CONFIG_TOKEN_KEY } from '../../../config/constants';
import { UserInfo } from '../../../models/user-info';
import { Roles } from '../../../shared/roles';
import { TranslateService } from '@ngx-translate/core';
import { InspectionCenterUiService } from 'inspection-center-ui';
import {Idle, DEFAULT_INTERRUPTSOURCES} from '@ng-idle/core';

@Component({
  selector: 'new-header',
  templateUrl: './new-header.component.html'
})
export class NewHeaderComponent implements OnInit {

  private navClasses = {
    normal: 'wrapper-collapse',
    collapsed: 'wrapper-expand'
  };

  private state = 'normal';
  userInfo: UserInfo = new UserInfo();
  userCenterText: string;
  appRoles = Roles;

  constructor(
    private authService: AuthService,
    private http: HttpClient,
    private el: ElementRef,
    private translate: TranslateService,
    private inspectionCenterUiService: InspectionCenterUiService,
    private idle: Idle,
    @Inject(CONFIG_TOKEN_KEY) private _config: any
  ) { }

  toggleNavbar() {
    let bodyClass = '';

    if (this.state === 'normal') {
      this.state = 'collapse';
      bodyClass = this.navClasses.collapsed;
    } else {
      this.state = 'normal';
      bodyClass = this.navClasses.normal;
    }

    this.el.nativeElement.closest('body').className = bodyClass;
    setTimeout(() => { window.dispatchEvent(new Event('resize')); }, 400);
  }

  ngOnInit() {
    const userClaims = this.authService.getClaims();
    this.userInfo = userClaims;
    this.userInfo.role = userClaims.role;
    this.updateUserCenterText();

    this.idle.setIdle(10 * 60);
    this.idle.setTimeout(1 * 60);
    // sets the default interrupts, like clicks, scrolls, touches to the document
    this.idle.setInterrupts(DEFAULT_INTERRUPTSOURCES);

    this.idle.onIdleStart.subscribe(() => {
      //TODO: add alert
    });

    this.idle.onTimeout.subscribe(() => {
      this.onSignOut();
    });

    this.idle.watch();
  }

  hasPrivilege(...roles: string[]) {
    if (Array.isArray(this.userInfo.role)) {
      const result = this.userInfo.role.filter((role) => roles.indexOf(role) >= 0);
      if (result.length > 0) {
        return true;
      }
    } else {
      return roles.indexOf(this.userInfo.role) >= 0;
    }
  }

  onSignOut() {
    this.authService.signOut();
  }

  updateUserCenterText() {
    if (this.userInfo && this.userInfo.inspection_center_id) {
      this.inspectionCenterUiService.get(this.userInfo.inspection_center_id).subscribe(center => {
        this.userCenterText = this.translate.instant('Inspection Center of') + ' ' + (typeof(center) === "string" ? JSON.parse(center) : center).name;
      });
    } else if (this.userInfo) {
      this.userCenterText = this.translate.instant('System Admin');
    }
  }
}
