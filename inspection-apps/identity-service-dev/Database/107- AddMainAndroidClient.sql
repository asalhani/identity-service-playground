--*****************************************************************************************
--** Add Android App Client
--*****************************************************************************************
DECLARE @ClientNameId nvarchar(200)
DECLARE @ClientName nvarchar(200)

DECLARE @ClientId INT

SET @ClientNameId = N'android'
SET @ClientName = N'Android Client Without Gateway'


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
1, @ClientNameId, N'oidc', 0, @ClientName, NULL, NULL, NULL, 0, 1, 
0, 0, 1, 
1, NULL, 1, 
NULL, 1, 1, 
300, 36000, 300, 
NULL, 2592000, 1296000, 
0, 0, 1, 
0, 1, 0, 
0, N'client_', NULL, 
GETDATE(), NULL, NULL, 
NULL, NULL, 300, 0)


SET @ClientId=@@IDENTITY

INSERT [IdentityService].[ClientGrantTypes] ([ClientId], [GrantType]) VALUES (@ClientId, N'authorization_code')

INSERT [IdentityService].[ClientScopes] ([ClientId], [Scope]) VALUES (@ClientId, N'openid')
INSERT [IdentityService].[ClientScopes] ([ClientId], [Scope]) VALUES (@ClientId, N'profile')
INSERT [IdentityService].[ClientScopes] ([ClientId], [Scope]) VALUES (@ClientId, N'inspection_profile')
INSERT [IdentityService].[ClientScopes] ([ClientId], [Scope]) SELECT @ClientId, [Name] FROM [IdentityService].[ApiScopes] WHERE  [NAME] not like '%.%'

INSERT [IdentityService].[ClientPostLogoutRedirectUris] ([ClientId], [PostLogoutRedirectUri]) VALUES (@ClientId, N'com.isp.app:/oauth2callbacklogout')
INSERT [IdentityService].[ClientRedirectUris] ([ClientId], [RedirectUri]) VALUES (@ClientId, N'com.isp.app:/oauth2callback')
INSERT [IdentityService].[ClientRedirectUris] ([ClientId], [RedirectUri]) VALUES (@ClientId, N'com.isp.app://oauth2callback')
