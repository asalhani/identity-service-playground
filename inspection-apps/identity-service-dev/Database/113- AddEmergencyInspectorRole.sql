IF NOT EXISTS (SELECT ID FROM [IdentityService].[AspNetRoles] WHERE NAME='EmergencyInspector')
BEGIN
INSERT INTO [IdentityService].[AspNetRoles]([Id],[Name],[NormalizedName],[ConcurrencyStamp])
		VALUES 
		(NEWID(), 'EmergencyInspector', 'EMERGENCYINSPECTOR', NEWID()) 
END