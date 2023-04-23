import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { IdentityUiComponent } from './identity-ui.component';
import { LoginViewComponent } from './views/login-view/login-view.component';
import { FormsModule } from '@angular/forms';
import { NgxCaptchaModule } from 'ngx-captcha';
import { OtpPartComponent } from './views/shared/otp-part.component';
import { LoginPartComponent } from './views/login-view/login-part.component';
import { ForgotPasswordViewComponent } from './views/forgot-password-view/forgot-password-view.component';
import { IdentityUiRoutingModule } from './identity-ui-routing.module';
import { ResetPasswordViewComponent } from './views/reset-password-view/reset-password-view.component';
import { EqualValidator } from '../shared/validators/equal-validator';
import { IdentityUiConfig, CONFIG_TOKEN_KEY } from './utils/identity-ui-config';
import { LogoutViewComponent } from './views/logout-view/logout-view.component';
import { PopoverModule } from 'ngx-popover';
import { ChangePasswordPartComponent } from './views/shared/change-password-part.component';
import { TranslateModule, TranslateLoader, TranslateService } from '@ngx-translate/core';
import { LanguageConfig } from './utils/language-config';
import { CustomTranslationLoader } from './utils/custom-translation-loader';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,
    TranslateModule.forChild({
      loader: {
        provide: TranslateLoader,
        useClass: CustomTranslationLoader,
        deps: [HttpClient, LanguageConfig]
      },
      isolate: true
    }),
    IdentityUiRoutingModule,
    // BrowserAnimationsModule,
    // SharedModule,
    NgxCaptchaModule,
    PopoverModule
  ],
  declarations: [
    IdentityUiComponent,
    LoginViewComponent,
    LoginPartComponent,
    OtpPartComponent,
    ForgotPasswordViewComponent,
    ResetPasswordViewComponent,
    EqualValidator,
    LogoutViewComponent,
    ChangePasswordPartComponent],
  exports: [
    TranslateModule
    // IdentityUiComponent,
    // LoginViewComponent
  ]
})
export class IdentityUiModule {

  constructor(translate: TranslateService) {
    translate.setDefaultLang(LanguageConfig.DEFAULT_LANG);
    translate.use(LanguageConfig.DEFAULT_LANG);
  }

  static forRoot(config: IdentityUiConfig = {}): ModuleWithProviders {
    return {
      ngModule: IdentityUiModule,
      providers: [
        { provide: CONFIG_TOKEN_KEY, useValue: config }
      ]
    };
  }
}
