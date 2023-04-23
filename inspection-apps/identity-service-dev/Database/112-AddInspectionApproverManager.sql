IF NOT EXISTS (SELECT ID FROM [IdentityService].[AspNetRoles] WHERE NAME='InspectionApproverManager')
BEGIN
INSERT INTO [IdentityService].[AspNetRoles]([Id],[Name],[NormalizedName],[ConcurrencyStamp])
		VALUES 
		(NEWID(), 'InspectionApproverManager', 'INSPECTIONAPPROVERMANAGER', NEWID()) 
END