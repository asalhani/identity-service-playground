import { Inject, Injectable } from '@angular/core';
import { IdentityUiConfig, CONFIG_TOKEN_KEY } from './identity-ui-config';

@Injectable({
  providedIn: 'root'
})
export class LanguageConfig {
  static DEFAULT_LANG = 'ar';

  constructor(@Inject(CONFIG_TOKEN_KEY) private _config: IdentityUiConfig) { }

  getTranslationUrl(lang: string, serviceUrlKey = 'identityServerEndpoint') {
    const url = this._config.getConfigValues()[serviceUrlKey] || '';
    return url.replace('api', '') + lang + '.json';
  }
}
