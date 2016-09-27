USE [JobManager]
GO

/****** Object:  StoredProcedure [dbo].[GS_GetJobIdByName]    Script Date: 21/09/2016 14:17:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- exec GS_InsertRecordJob @jobName='XX1',@MaxRetries=45,@ModelXml='<root></root>',@ModelTypeXml='t'
-- EXEC GS_GetJobIdByName 'XX'
CREATE PROCEDURE [dbo].[GS_GetJobIdByName]
	@jobName nvarchar(300),@MaxRetries int=10
AS
BEGIN
	
	SET NOCOUNT ON;
	DECLARE @JobId uniqueidentifier;
	EXEC GS_JobAddIfNotExist @JobId=@JobId output,@MaxRetries=@MaxRetries ,@jobName= @jobName;
	
END

GO


