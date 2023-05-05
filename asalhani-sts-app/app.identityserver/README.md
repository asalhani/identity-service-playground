## Run migration command
```shell
dotnet ef database update --context ApplicationDbContext
dotnet ef database update --context PersistedGrantDbContext
dotnet ef database update --context ConfigurationDbContext
```

---------------

### What is an Api Resource?
> Anything that you wish to protect using the Identity Server is considered as an API Resource.
> An API Resource is something the identity server protects.

### What is an API Scope?
> The Identity Server will check for incoming requests and validate that the scope being requested matches up with the API’s allowed scopes. If a scope isn’t found, or isn’t assigned to the consumer, an ‘invalid_scope’ error will be returned.

Likewise, for our Catalog API, we have 2 distinct overarching scopes: read-only operations and admin operations. You can essentially define these as API Scopes.

So, I can easily create new scopes called: catalog.readonly and catalog.admin that I can potentially distribute to consumers depending on the needs.

### What is an API Claim?
> An API Claim is a User Claim that will be included in the Access Token.

> The scope defines what I can do at an API level. The Claim will define what I can do at a user level.

Let’s say that we have a permission called ‘delete-countries’ which we need to check for before we delete a country. Even though we have Admin capabilities we have granted at the API level using ‘catalog.admin’ scope, we can still check for a ‘delete-countries’ claim to ensure that the user has delete rights.

### What is an API Scope Claim?
> An API Scope Claim is simply **a User Claim** that can be returned with a given **scope**.

> If you wanted claims to only be returned when specific scopes were requested, you could set them up in the API Scope Claims table

### What is an API Secret?
> An API Secret is simply as secret that you can use to introspect tokens.

### What are Clients?
> Clients are a means to grant access to resources.

> Clients could be Machines, Browsers or Native Applications anonymously accessing resources or a on-behalf of a user logged in requesting access to the protected resources.

| Property          	| Definition                                                                                                                                                               	|
|-------------------	|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------	|
| client id         	| to uniquely identify the client                                                                                                                                          	|
| secret            	| if needed to establish access. In unsafe scenarios, a secret can be optional, provided there’s a                                                                         	|
| redirect_uri      	| for the identity server to direct the token information to. This helps when secrets cannot be divulged. Especially in Javascript apps where nothing is a secret.         	|
| scopes            	| request the rights of what the client can have access to.                                                                                                                	|
| response type     	| if set, will allow defining the expected token type.                                                                                                                     	|
| offline access    	| will return a refresh token in return that will allow the access tokens to be refreshed over and over again.                                                             	|
| token lifetimes   	| usually, depending on the type of the client, you can change the lifetime of various tokens (namely id tokens, access tokens, refresh tokens, authorization codes etc.). 	|
| CORS restrictions 	| I could setup access to specific resources from specific origins only.                                                                                                   	|

#### What is a Grant Type?
> A grant type defines the mechanism of communicating with a Client. There are numerous different grant types to choose from

| Grant Type        	  | Definition                                                                                                                                                                                                                                                                                                                                                      	|
|----------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------	|
| Client Credentials 	 | This is primarily used for M2M and in environments where we know that the chances of exposing a password (or a secret) are relatively low.                                                                                                                                                                                                                      	|
| Implicit           	 | This is primarily used in Javascript browser environments where there’s no such thing as secrets. This approach requires the Identity Server to call back a pre-configured URL that it can post the secrets to in real-time. In implicit flows, tokens (or more specifically an Id Token) are passed to the browser directly.                                   	|
| Hybrid             	 | Provides best of both worlds. One can request Id Token using a browser, and request a more detailed token called “access token” using a back-channel server channel.                                                                                                                                                                                            	|
| Password           	 | a resource owner passes his username and password to the Identity server. The identity server then validates the user and then grants a token. This approach should be used sparingly. Mostly used in scenarios where one doesn’t want to redirect the user to an Identity Server for login and instead pass login credentials directly to the Identity server. 	|

