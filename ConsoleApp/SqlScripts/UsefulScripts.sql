-- Transaction with error handling and rollback
BEGIN TRY
	BEGIN TRAN t

	-- ...

	COMMIT TRAN t
END TRY
BEGIN CATCH
	ROLLBACK TRAN t

	DECLARE @ErrorMessage NVARCHAR(4000);
	DECLARE @ErrorSeverity INT;
	DECLARE @ErrorState INT;

	SELECT 
		@ErrorMessage = ERROR_MESSAGE(),
		@ErrorSeverity = ERROR_SEVERITY(),
		@ErrorState = ERROR_STATE();

	RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
END CATCH

-- Declare variable
DECLARE @Limit INT = 1000;

-- Set identity insert on/off
SET IDENTITY_INSERT dbo.Tool ON;
SET IDENTITY_INSERT dbo.Tool OFF;

-- Reseed/reset identity insert/increment
DBCC CHECKIDENT('[VISA_TRANSACTIONS]', RESEED, 0);