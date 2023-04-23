import { Injectable, Inject } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import { CONFIG_TOKEN_KEY } from '../../config/constants';

@Injectable({
  providedIn: 'root'
})
export class InboxWidgetService {

  triggerInboxRefershSource = new Subject<boolean>();
  triggerInboxRefersh$ = this.triggerInboxRefershSource.asObservable();

  private baseUrl: string;
  private endpoint = 'inbox';
  private inboxCountEndpoint = 'userInboxCount';
  private opts = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };

  constructor(private http: HttpClient, @Inject(CONFIG_TOKEN_KEY) private _config: { getConfigValue: (key: string) => string }) {
    this.configure();
  }

  getInboxCount(): Observable<number> {
    return this.http.get<number>(`${this.endpoint}/${this.inboxCountEndpoint}`, this.opts);
  }

  private configure() {
    const serviceUrl = this._config.getConfigValue('InspectionProcessServiceURL');
    this.baseUrl = serviceUrl ? serviceUrl : '';
    this.endpoint = `${this.baseUrl}/${this.endpoint}`;
  }
}
