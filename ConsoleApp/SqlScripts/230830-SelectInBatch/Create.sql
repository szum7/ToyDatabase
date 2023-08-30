DECLARE @Limit INT = 1000;

BEGIN TRY
	BEGIN TRAN t

	CREATE TABLE VISA_TRANSACTIONS (
		[ID] INT IDENTITY(1, 1) NOT NULL,
		[ID_NO_INDEX] INT NOT NULL,
		[TITLE] NVARCHAR(255) NULL,
		[AMOUNT] INT NOT NULL,
		CONSTRAINT [PK_VISA_TRANSACTIONS] PRIMARY KEY CLUSTERED
		(
			[ID] ASC
		)
		WITH (
			PAD_INDEX = OFF, 
			STATISTICS_NORECOMPUTE = OFF, 
			IGNORE_DUP_KEY = OFF, 
			ALLOW_ROW_LOCKS = ON, 
			ALLOW_PAGE_LOCKS = ON 
			--OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF -- unsupported
		) ON [PRIMARY]
	) ON [PRIMARY]

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