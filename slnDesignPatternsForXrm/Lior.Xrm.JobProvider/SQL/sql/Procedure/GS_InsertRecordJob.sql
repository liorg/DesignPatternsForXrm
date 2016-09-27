USE [JobManager]
GO
/****** Object:  StoredProcedure [dbo].[GS_InsertRecordJob]    Script Date: 26/09/2016 15:02:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec GS_InsertRecordJob @jobName='XX1',@MaxRetries=45,@ModelXml='<root>3</root>',@ModelTypeXml='t'
-- EXEC GS_JobAddIfNotExist 'XX'
ALTER PROCEDURE [dbo].[GS_InsertRecordJob]
	@jobName nvarchar(300) ,@historyId uniqueidentifier, @MaxRetries int=10,@Id uniqueidentifier=null, @ModelXml xml='',@StatusId int=0,@ModelTypeXml nvarchar(500)='',@Retry int =0
AS
BEGIN
	
	SET NOCOUNT ON;
	declare @JobId uniqueidentifier;

	exec GS_JobAddIfNotExist @JobId =@JobId output,@MaxRetries=@MaxRetries ,@jobName= @jobName;
	if(@Id is null)
	begin
	INSERT INTO dbo.Records
		(ModelXml, StatusId, CreatedOn, ModifiedOn, Retry, JobId, ModelType,historyId)
     VALUES
           (@ModelXml ,@StatusId,GETDATE() ,GETDATE()  ,@Retry  ,@JobId  ,@ModelTypeXml,@historyId)
   end
   select @JobId
END
