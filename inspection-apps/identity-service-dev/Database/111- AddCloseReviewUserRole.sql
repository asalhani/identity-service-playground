IF NOT EXISTS (SELECT ID FROM [IdentityService].[AspNetRoles] WHERE NAME='CloseReviewUser')
BEGIN
INSERT INTO [IdentityService].[AspNetRoles]([Id],[Name],[NormalizedName],[ConcurrencyStamp])
		VALUES 
		(NEWID(), 'CloseReviewUser', 'CLOSEREVIEWUSER', NEWID()) 
END