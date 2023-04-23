import { Component, OnInit, OnDestroy } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { DEFAULT_LANG } from '../../../config/constants';
import { AuthService } from 'identity-guards';
import { UserInfo } from '../../../models/user-info';
import { Roles } from '../../../../app/shared/roles';
import { AppConfigService } from '../../../../app/config/app-config.service';
import { InboxWidgetService } from '../../services/inbox-widget.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-new-nav',
  templateUrl: './new-nav.component.html',
  styles: [`
    .custom-badge {
      background-color: #75BB25;
      color: #ffffff;
    }
  `]
})
export class NewNavComponent implements OnInit, OnDestroy {

  userInfo: UserInfo = new UserInfo();
  appRoles = Roles;
  appTitle: string;
  appLogo: string;
  userInboxCount: number;
  subscription: Subscription;

  constructor(
    private translate: TranslateService,
    private authService: AuthService,
    private _inboxWidgetService: InboxWidgetService
    ) {
    translate.setDefaultLang(DEFAULT_LANG);
    translate.use(DEFAULT_LANG);
  }

  ngOnInit() {
    const userClaims = this.authService.getClaims();

    if (Array.isArray(userClaims.given_name)) {
      this.userInfo.name = userClaims.given_name[0];
    } else {
      this.userInfo.name = userClaims.given_name;
    }

    this.userInfo.role = userClaims.role;
    this.appTitle = AppConfigService.settings.AppTitle;
    this.appLogo = AppConfigService.settings.AppLogo;

    this.fetchInboxCount();
    this.subsribeOnInboxRefreshes();
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
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

  private fetchInboxCount() {
    this._inboxWidgetService.getInboxCount().subscribe((result) => {
      this.userInboxCount = result;
    }, (error) => { });
  }

  private subsribeOnInboxRefreshes() {
    this.subscription = this._inboxWidgetService.triggerInboxRefersh$.subscribe((_) => {
      this.fetchInboxCount();
    });
  }
}
