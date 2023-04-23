IF NOT EXISTS (SELECT ID FROM [IdentityService].[AspNetRoles] WHERE NAME='HelpDeskAdmin')
BEGIN
INSERT INTO [IdentityService].[AspNetRoles]([Id],[Name],[NormalizedName],[ConcurrencyStamp])
		VALUES 
		(NEWID(), 'HelpDeskAdmin', 'HELPDESKADMIN', NEWID()) 
END