DECLARE @ApiResourcesId INT
DECLARE @ApiScopeId INT
DECLARE @Description NVARCHAR(1000)
DECLARE @ServiceCode NVARCHAR(200)

--*****************************************************************************************
--** Entity Service
--*****************************************************************************************
SET @Description=N'Entity Service'
SET @ServiceCode=N'entityservice'

INSERT [IdentityService].[ApiResources] ([Enabled], [Name], [DisplayName], [Description], [Created], [Updated], [LastAccessed], [NonEditable]) VALUES (1, @ServiceCode, @Description, NULL, GETDATE(), NULL, NULL, 0)
SET @ApiResourcesId=@@IDENTITY
INSERT [IdentityService].[ApiScopes] ([ApiResourceId], [Description], [DisplayName], [Emphasize], [Name], [Required], [ShowInDiscoveryDocument]) VALUES (@ApiResourcesId, NULL, @Description, 0, @ServiceCode, 0, 1)
INSERT [IdentityService].[ApiScopes] ([ApiResourceId], [Description], [DisplayName], [Emphasize], [Name], [Required], [ShowInDiscoveryDocument]) VALUES (@ApiResourcesId, NULL, @Description, 0, @ServiceCode+N'.internal', 0, 0)
INSERT [IdentityService].[ApiClaims] ([ApiResourceId], [Type]) VALUES (@ApiResourcesId, N'name')
INSERT [IdentityService].[ApiClaims] ([ApiResourceId], [Type]) VALUES (@ApiResourcesId, N'given_name')
INSERT [IdentityService].[ApiClaims] ([ApiResourceId], [Type]) VALUES (@ApiResourcesId, N'family_name')
INSERT [IdentityService].[ApiClaims] ([ApiResourceId], [Type]) VALUES (@ApiResourcesId, N'phone_number')
INSERT [IdentityService].[ApiClaims] ([ApiResourceId], [Type]) VALUES (@ApiResourcesId, N'email')
INSERT [IdentityService].[ApiClaims] ([ApiResourceId], [Type]) VALUES (@ApiResourcesId, N'inspection_center_id')
INSERT [IdentityService].[ApiClaims] ([ApiResourceId], [Type]) VALUES (@ApiResourcesId, N'role')

--*****************************************************************************************
--** Elm BPM Service
--*****************************************************************************************
SET @Description=N'Elm BPM Service'
SET @ServiceCode=N'elmbpmservice'

INSERT [IdentityService].[ApiResources] ([Enabled], [Name], [DisplayName], [Description], [Created], [Updated], [LastAccessed], [NonEditable]) VALUES (1, @ServiceCode, @Description, NULL, GETDATE(), NULL, NULL, 0)
SET @ApiResourcesId=@@IDENTITY
INSERT [IdentityService].[ApiScopes] ([ApiResourceId], [Description], [DisplayName], [Emphasize], [Name], [Required], [ShowInDiscoveryDocument]) VALUES (@ApiResourcesId, NULL, @Description, 0, @ServiceCode, 0, 1)
INSERT [IdentityService].[ApiScopes] ([ApiResourceId], [Description], [DisplayName], [Emphasize], [Name], [Required], [ShowInDiscoveryDocument]) VALUES (@ApiResourcesId, NULL, @Description, 0, @ServiceCode+N'.internal', 0, 0)
INSERT [IdentityService].[ApiClaims] ([ApiResourceId], [Type]) VALUES (@ApiResourcesId, N'name')
INSERT [IdentityService].[ApiClaims] ([ApiResourceId], [Type]) VALUES (@ApiResourcesId, N'given_name')
INSERT [IdentityService].[ApiClaims] ([ApiResourceId], [Type]) VALUES (@ApiResourcesId, N'family_name')
INSERT [IdentityService].[ApiClaims] ([ApiResourceId], [Type]) VALUES (@ApiResourcesId, N'phone_number')
INSERT [IdentityService].[ApiClaims] ([ApiResourceId], [Type]) VALUES (@ApiResourcesId, N'email')
INSERT [IdentityService].[ApiClaims] ([ApiResourceId], [Type]) VALUES (@ApiResourcesId, N'inspection_center_id')
INSERT [IdentityService].[ApiClaims] ([ApiResourceId], [Type]) VALUES (@ApiResourcesId, N'role')


--*****************************************************************************************
--** Core Services Proxy
--*****************************************************************************************
SET @Description=N'Core Services Proxy'
SET @ServiceCode=N'coreservicesproxy'

INSERT [IdentityService].[ApiResources] ([Enabled], [Name], [DisplayName], [Description], [Created], [Updated], [LastAccessed], [NonEditable]) VALUES (1, @ServiceCode, @Description, NULL, GETDATE(), NULL, NULL, 0)
SET @ApiResourcesId=@@IDENTITY
INSERT [IdentityService].[ApiScopes] ([ApiResourceId], [Description], [DisplayName], [Emphasize], [Name], [Required], [ShowInDiscoveryDocument]) VALUES (@ApiResourcesId, NULL, @Description, 0, @ServiceCode, 0, 1)
INSERT [IdentityService].[ApiScopes] ([ApiResourceId], [Description], [DisplayName], [Emphasize], [Name], [Required], [ShowInDiscoveryDocument]) VALUES (@ApiResourcesId, NULL, @Description, 0, @ServiceCode+N'.internal', 0, 0)
INSERT [IdentityService].[ApiClaims] ([ApiResourceId], [Type]) VALUES (@ApiResourcesId, N'name')
INSERT [IdentityService].[ApiClaims] ([ApiResourceId], [Type]) VALUES (@ApiResourcesId, N'given_name')
INSERT [IdentityService].[ApiClaims] ([ApiResourceId], [Type]) VALUES (@ApiResourcesId, N'family_name')
INSERT [IdentityService].[ApiClaims] ([ApiResourceId], [Type]) VALUES (@ApiResourcesId, N'phone_number')
INSERT [IdentityService].[ApiClaims] ([ApiResourceId], [Type]) VALUES (@ApiResourcesId, N'email')
INSERT [IdentityService].[ApiClaims] ([ApiResourceId], [Type]) VALUES (@ApiResourcesId, N'inspection_center_id') 
INSERT [IdentityService].[ApiClaims] ([ApiResourceId], [Type]) VALUES (@ApiResourcesId, N'role')


GO
