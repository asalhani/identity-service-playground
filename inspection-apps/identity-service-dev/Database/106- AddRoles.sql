

INSERT INTO [IdentityService].[AspNetRoles]([Id],[Name],[NormalizedName],[ConcurrencyStamp])
		VALUES 
		(NEWID(), 'SystemAdmin', 'SYSTEMADMIN', NEWID()),
		(NEWID(), 'CenterAdmin', 'CENTERADMIN', NEWID()),
		(NEWID(), 'Inspector', 'INSPECTOR', NEWID()),
		(NEWID(), 'InspectionApprover', 'INSPECTIONAPPROVER', NEWID()),
		(NEWID(), 'AppealReviewer', 'APPEALREVIEWER', NEWID()),
		(NEWID(), 'HelpDesk', 'HELPDESK', NEWID()),
		(NEWID(), 'FieldSupporter', 'FIELDSUPPORTER', NEWID()),
		(NEWID(), 'FieldSupportManager', 'FIELDSUPPORTMANAGER', NEWID()),
		(NEWID(), 'SystemManager', 'SYSTEMMANAGER', NEWID()),
		(NEWID(), 'EmergencyManager', 'EMERGENCYMANAGER', NEWID()),
		(NEWID(), 'Surveyor', 'SURVEYOR', NEWID()),
		(NEWID(), 'SurveyorManager', 'SURVEYORMANAGER', NEWID())