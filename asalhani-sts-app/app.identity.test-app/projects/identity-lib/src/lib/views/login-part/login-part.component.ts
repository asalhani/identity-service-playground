import {Component, EventEmitter, Inject, Input, OnInit, Output} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {CONFIG_TOKEN_KEY, IdentityUiConfig} from "../../utils/identity-ui-config";
import {AuthenticationServerService} from "../../services/authentication-server-service";
import {LoginBaseView} from "../login-base-view";
import {PublicUserLoginModel} from "../../models/user-login-model";


@Component({
  selector: 'lib-login-part',
  templateUrl: './login-part.component.html',
  styleUrls: ['./login-part.component.css']
})
export class LoginPartComponent extends  LoginBaseView implements OnInit {


  constructor(@Inject(CONFIG_TOKEN_KEY) private _config: IdentityUiConfig
              , private formBuilder: FormBuilder
              , private _authService: AuthenticationServerService) {
    super();
  }

  override ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      idNumber: ['', Validators.compose([Validators.required/*, AwqafValidators.ninOrIqama(IdType.NIN_OR_IQAMA)*/])],
    });
  }

  onLogin(formIsValid: boolean) {
    if (formIsValid) {
      this.loginData = new class implements PublicUserLoginModel {
        loginName: string;
      };
      this.loginData.loginName = this.loginForm.controls['idNumber'].value;
      this._authService.authenticatePublicUser(this.loginData).subscribe(result => {
        this.handleLoginResponse(result);
      }, error => {
        console.error('Fetching data from the server failed. Please try again.');
      });
    }
  }
}
