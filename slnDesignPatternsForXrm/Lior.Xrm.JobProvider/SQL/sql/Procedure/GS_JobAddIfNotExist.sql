USE [JobManager]
GO

/****** Object:  StoredProcedure [dbo].[GS_JobAddIfNotExist]    Script Date: 21/09/2016 14:19:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- declare @JobId uniqueidentifier;exec GS_JobAddIfNotExist @JobId =@JobId output,@MaxRetries=56 ,@jobName= 'sggx'; 
CREATE PROCEDURE [dbo].[GS_JobAddIfNotExist]
	-- Add the parameters for the stored procedure here
	 @JobId  uniqueidentifier output,@jobName nvarchar(300),@MaxRetries int =null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	--declare @JobId uniqueidentifier;
	DECLARE @MaxRetriesTemp int 
	SELECT @JobId=[JobId] ,@MaxRetriesTemp=[MaxRetries]
	FROM [dbo].[Jobs]
	WHERE [JobName]=@jobName

declare @op table
(
    ColGuid uniqueidentifier
)

  if(@JobId is null)
  BEGIN
   if (@MaxRetries is not null)
     set @MaxRetriesTemp=@MaxRetries;
   else
     set @MaxRetriesTemp=10;

	INSERT INTO [dbo].[Jobs]		([JobName] ,[MaxRetries])	
	OUTPUT INSERTED.JobId  into @op
	VALUES  (@jobName ,@MaxRetriesTemp)
	
	SELECT @JobId= o.ColGuid from @op o
    SET @MaxRetries=@MaxRetriesTemp;
  END
  ELSE
  BEGIN  
  if (@MaxRetries is not null)
  BEGIN
	  UPDATE [dbo].[Jobs]
		SET [MaxRetries] = @MaxRetries
		WHERE [JobId]=@JobId
  END	  
  ELSE 
	BEGIN 
	 SET @MaxRetries=@MaxRetriesTemp;
	END 
  END
  SELECT @JobId,@MaxRetries
END

GO


