USE [JobManager]
GO
/****** Object:  StoredProcedure [dbo].[GS_getRecordsByJobId]    Script Date: 29/09/2016 11:39:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

--exec [dbo].[GS_getRecordsByJobId] 'de9a6f4e-695d-41fa-bae7-d4965a49c380'
ALTER PROCEDURE [dbo].[GS_getRecordsByJobId]
	@JobId uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;
	SELECT  R.ID,
			R.ModelXml,
			R.StatusId,
			R.CreatedOn,
			R.ModifiedOn,
			R.Retry,
			R.JobId,
			R.ModelType
	FROM dbo.Records R
	INNER JOIN dbo.Jobs J
	

		ON R.JobId=J.JobId
		inner join dbo.JobHistory jh
		on jh.HistoryId=r.HistoryId or jh.HistoryId='00000000-0000-0000-0000-000000000000'
	WHERE R.JobId = @JobId AND 
	R.Retry < J.MaxRetries 
	AND (R.StatusId = 0 or R.StatusId = 4) and (jh.IsGetDataComplete=1 or jh.HistoryId='00000000-0000-0000-0000-000000000000')

 
END
