USE [JobManager]
GO
/****** Object:  StoredProcedure [dbo].[GS_getRecordsByJobId]    Script Date: 31/10/2016 16:58:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

--	exec [dbo].[GS_getRecordsByJobId] @JobId='25eeb0e0-2fb4-4456-9ae1-fa119a5ee4b8' ,@PageNumber=2,@RowspPage=3
ALTER PROCEDURE [dbo].[GS_getRecordsByJobId]
	@JobId uniqueidentifier,@PageNumber  int =2,@RowspPage int =5000 
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
	order by CreatedOn
OFFSET ((@PageNumber - 1) * @RowspPage) ROWS FETCH NEXT @RowspPage ROWS ONLY;
END
