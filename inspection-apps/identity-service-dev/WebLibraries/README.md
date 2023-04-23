```
███████╗██╗     ███╗   ███╗
██╔════╝██║     ████╗ ████║
█████╗  ██║     ██╔████╔██║
██╔══╝  ██║     ██║╚██╔╝██║
███████╗███████╗██║ ╚═╝ ██║
╚══════╝╚══════╝╚═╝     ╚═╝
```
# Identity Packages
***

In this document:
* **Identity UI**: Contains the components: login, forgot password, and reset password.
* **Identity Guards**: Provides route protection functionality.

## Commands Userd to Generate these Libraries
```
ng new identity-app --directory ./
ng g library identity-ui -p idensrv
ng g component login-view --project=identity-ui
ng g component login-view --project=identity-guards
```

To build library in development use:
```
ng build identity-ui
ng build identity-guards
```
To build library for production use:
```
ng build identity-ui --prod
ng build identity-guards --prod
```
To install the generated package with yarn:
```
yarn add file:./dist/identity-ui
yarn add file:./dist/identity-guards
```

# Identity UI
***

## Importing `identity-ui` Package
Reference the package in your `package.json` file and then install the package with `yarn` or `npm`
```
dependencies:{
  ....
  "identity-ui": "file:./dist/identity-ui" or "~<package version>"
  ....
}
```

Import `IdentityUiModule` in your app module and pass the required configurations
```
import { IdentityUiModule } from 'identity-ui'

@NgModule({
imports: [
  ....
  IdentityUiModule.forRoot({ 
    recaptchaSiteKey: "6LeIxAcTAAAAAJcZVRqyHh71UMIEGNQ_MXjiZKhI",
    identityServerEndpoint: "http://localhost:5000"
  }),
  ....
]
```
Import `IdentityUiRoutingModule` in your routing module
```
import { IdentityUiRoutingModule } from 'identity-ui'
```
Add path entry (e.g. `identity-ui`) to your main routing table:
```
const routes: Routes = [
  { path: 'identity-ui', children: IdentityUiRoutingModule.getRoutes() }
]
```
Test by navigating to: `/identity-ui/forgot-password`

# Identity Guards
***

## Importing `identity-guards` Package
Reference the package in your `package.json` file and then install the package with `yarn` or `npm`
```
dependencies:{
  ....
  "identity-guards": "file:./dist/identity-guards" or "~<package version>"
  ....
}
```

Import `IdentityGuardsModule` in your app module and pass the required configurations
```
import { IdentityGuardsModule } from 'identity-guards';

@NgModule({
imports: [
  ....
  IdentityGuardsModule.forRoot({
    oidcSettings: {
      authority: 'http://localhost:5000/',
      client_id: 'angular_spa',
      redirect_uri: 'http://localhost:4200/#/identity-guards/auth-callback#',
      post_logout_redirect_uri: 'http://localhost:4200/',
      response_type: "id_token token",
      scope: "openid profile",
      filterProtocolClaims: true,
      loadUserInfo: true
    }
  }),
  ....
]
```

Import `AuthGuardService` and `IdentityGuardsRoutingModule` in your routing module
```
import { AuthGuardService, IdentityGuardsRoutingModule } from 'identity-guards';
```
Add path entry (e.g. `identity-guards`) to your main routing table:
```
const routes: Routes = [
  { path: 'identity-guards', children: IdentityGuardsRoutingModule.getRoutes() }
]
```
Protect any route by assigning `AuthGuardService` to the `canActivate` property of your secure route entry
``` 
{
  path: 'protected-page',
  component: ProtectedPageComponent,
  canActivate: [AuthGuardService]
}
```
Start a new browser session or clear all cookies and test by navigating to a secure route.
