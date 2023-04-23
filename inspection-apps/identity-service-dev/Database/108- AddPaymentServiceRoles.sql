IF NOT EXISTS (SELECT ID FROM [IdentityService].[AspNetRoles] WHERE NAME='FinanceOfficer')
BEGIN
INSERT INTO [IdentityService].[AspNetRoles]([Id],[Name],[NormalizedName],[ConcurrencyStamp])
		VALUES 
		(NEWID(), 'FinanceOfficer', 'FINANCEOFFICER', NEWID())
END

IF NOT EXISTS (SELECT ID FROM [IdentityService].[AspNetRoles] WHERE NAME='FinanceAdmin')
BEGIN
INSERT INTO [IdentityService].[AspNetRoles]([Id],[Name],[NormalizedName],[ConcurrencyStamp])
		VALUES 
		(NEWID(), 'FinanceAdmin', 'FINANCEADMIN', NEWID())
END