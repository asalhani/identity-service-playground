# Medium publication
This repo contains source code described in the Medium publication  [Secure Angular 11/12 with IdentityServer4 Admin UI](https://medium.com/scrum-and-coke/secure-angular-with-identityserver4-admin-ui-5dd163ff1434)
# Example angular-oauth2-oidc with AuthGuard

This repository shows a basic Angular CLI application with [the `angular-oauth2-oidc` library](https://github.com/manfredsteyer/angular-oauth2-oidc) and Angular AuthGuards.

[![Lint-Build-Test GitHub Actions Status](https://github.com/jeroenheijmans/sample-angular-oauth2-oidc-with-auth-guards/workflows/Lint-Build-Test/badge.svg)](https://github.com/jeroenheijmans/sample-angular-oauth2-oidc-with-auth-guards/actions)


## Features


This demonstrates:

- Use of **the Code+PKCE Flow** (so no JWKS validation)
- Modules (core, shared, and two feature modules)
- An auth guard that forces you to login when navigating to protected routes
- An auth guard that just prevents you from navigating to protected routes
- Asynchronous loading of login information (and thus async auth guards)
- Using `localStorage` for storing tokens (use at your own risk!)
- Loading IDS details from its discovery document
- Trying refresh on app startup before potientially starting a login flow
- OpenID's external logout features


For new applications Code+PKCE flow is recommended for JavaScript clients, and this example repository now demonstrates this as the main use case.

## Usage

To use the repository:

1. Clone this repository
1. Run `npm install` to get the dependencies
1. Run `npm run start` (or `start-with-ssl`) to get it running on [http://localhost:4200](http://localhost:4200) (or [https://localhost:4200](https://localhost:4200))


You could also connect to your own IdentityServer by changing `auth-config.ts`.
Note that your server must whitelist both `http://localhost:4200/index.html` and `http://localhost:4200/silent-refresh.html` for this to work.

## Differences between Identity Server options

**This repository demonstrates features using https://demo.identityserver.io (IdentityServer4)**.
There are various other server side solutions available, each with their own intricacies.
This codebase does not keep track itself of the specifics for each other server side solution.
Instead, we recommend you look for specific guidance for other solutions elsewhere.
Here are some potential starting points you could consider:

- IdenitityServer4
  - This sample itself uses IDS4
- Auth0
  - [github.com/jeroenheijmans/sample-auth0-angular-oauth2-oidc](https://github.com/jeroenheijmans/sample-auth0-angular-oauth2-oidc): Angular 6 and Auth0 integration
- Keycloak
  - No samples or tutorials yet
- Okta
  - No samples or tutorials yet
- Microsoft AAD
  - No samples or tutorials yet
- ...

Feel free to open an issue and PR if you want to add additional pieces of guidance to this section.

## Example

The application is supposed to look somewhat like this:

![Application Screenshot](screenshot-001.png)
