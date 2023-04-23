GO

DECLARE @ClientUrl nvarchar(200)
DECLARE @ClientNameId nvarchar(200)
DECLARE @ClientName nvarchar(200)
DECLARE @Password varchar(4000)
DECLARE @HashThis nvarchar(4000)

DECLARE @ClientId INT
--*****************************************************************************************
--** Add SPA client
--*****************************************************************************************
SET @ClientUrl = N'$ClientUrl$'
--SET @ClientUrl = N'http://localhost:4200'
SET @ClientNameId =N'$ClientNameId$'
--SET @ClientNameId = N'local_spa'
SET @ClientName = N'$ClientName$'

INSERT [IdentityService].[Clients] (
[Enabled], [ClientId], [ProtocolType], [RequireClientSecret], [ClientName], [Description], [ClientUri], [LogoUri], [RequireConsent], [AllowRememberConsent], 
[AlwaysIncludeUserClaimsInIdToken], [RequirePkce], [AllowPlainTextPkce], 
[AllowAccessTokensViaBrowser], [FrontChannelLogoutUri], [FrontChannelLogoutSessionRequired], 
[BackChannelLogoutUri], [BackChannelLogoutSessionRequired], [AllowOfflineAccess], 
[IdentityTokenLifetime], [AccessTokenLifetime], [AuthorizationCodeLifetime], 
[ConsentLifetime], [AbsoluteRefreshTokenLifetime], [SlidingRefreshTokenLifetime], 
[RefreshTokenUsage], [UpdateAccessTokenClaimsOnRefresh], [RefreshTokenExpiration], 
[AccessTokenType], [EnableLocalLogin], [IncludeJwtId], 
[AlwaysSendClientClaims], [ClientClaimsPrefix], [PairWiseSubjectSalt], 
[Created], [Updated], [LastAccessed], 
[UserSsoLifetime], [UserCodeType], [DeviceCodeLifetime], [NonEditable]) 
VALUES (
1, @ClientNameId, N'oidc', 1, @ClientName, NULL, NULL, NULL, 0, 1, 
0, 0, 0, 
1, NULL, 1, 
NULL, 1, 0, 
300, 3600, 300, 
NULL, 2592000, 1296000, 
1, 0, 1, 
0, 1, 0, 
0, N'client_', NULL, 
GETDATE(), NULL, NULL, 
NULL, NULL, 300, 0)


SET @ClientId=@@IDENTITY

INSERT [IdentityService].[ClientCorsOrigins] ([ClientId], [Origin]) VALUES (@ClientId, @ClientUrl)
INSERT [IdentityService].[ClientGrantTypes] ([ClientId], [GrantType]) VALUES (@ClientId, N'implicit')

INSERT [IdentityService].[ClientScopes] ([ClientId], [Scope]) VALUES (@ClientId, N'openid')
INSERT [IdentityService].[ClientScopes] ([ClientId], [Scope]) VALUES (@ClientId, N'profile')
INSERT [IdentityService].[ClientScopes] ([ClientId], [Scope]) VALUES (@ClientId, N'inspection_profile')

INSERT [IdentityService].[ClientPostLogoutRedirectUris] ([ClientId], [PostLogoutRedirectUri]) VALUES (@ClientId, @ClientUrl)
INSERT [IdentityService].[ClientRedirectUris] ([ClientId], [RedirectUri]) VALUES (@ClientId, @ClientUrl+N'/#/identity-guards/auth-callback#')
INSERT [IdentityService].[ClientRedirectUris] ([ClientId], [RedirectUri]) VALUES (@ClientId, @ClientUrl+N'/#/identity-guards/silent-refresh#')
--INSERT [IdentityService].[ClientRedirectUris] ([ClientId], [RedirectUri]) VALUES (@ClientId, @ClientUrl+N'/silent-refresh.html')