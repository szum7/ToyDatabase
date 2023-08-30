SELECT [ID]
      ,[ID_NO_INDEX]
      ,[TITLE]
      ,[AMOUNT]
FROM [VisaIssueControl].[dbo].[VISA_TRANSACTIONS];

SELECT count(*) AS RecordCount 
FROM [VISA_TRANSACTIONS];

-- To check the sum and if the program was correct
SELECT sum(amount)
FROM visa_transactions;