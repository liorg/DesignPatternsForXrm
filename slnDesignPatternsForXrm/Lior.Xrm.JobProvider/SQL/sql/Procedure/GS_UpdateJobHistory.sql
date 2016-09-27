USE [JobManager]
GO
/****** Object:  StoredProcedure [dbo].[GS_UpdateJobHistory]    Script Date: 26/09/2016 13:22:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROC [dbo].[GS_UpdateJobHistory]
@id uniqueidentifier,@IsGetDataComplete bit=null,
@total int=null, @noupdate int=null,
@update int=null,@insert int=null,
@success int=null,
@failed int=null
AS

BEGIN
DECLARE @op TABLE
(
    ColGuid uniqueidentifier
)

UPDATE dbo.JobHistory
   SET EndedAt = getdate()
      ,TotalRecords = isnull(@total,TotalRecords)
      ,NoUpdated =  isnull(@noupdate,NoUpdated)
      ,Updated =  isnull(@update,Updated)
      ,Inserted = isnull(@insert,Inserted)
      ,Succeeded = isnull(@success,Succeeded)
      ,Failed = isnull(@failed,Failed)
	  ,IsGetDataComplete=isnull(@IsGetDataComplete,IsGetDataComplete)
      ,[Status] = 2
 WHERE HistoryId = @id

END
