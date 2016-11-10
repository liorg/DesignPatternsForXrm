USE [JobManager]
GO
/****** Object:  StoredProcedure [dbo].[GS_getRecordsByJobId]    Script Date: 09/11/2016 16:31:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

--	exec [dbo].[GS_getRecordsByJobId] @JobId='A71DBADC-1DFF-4134-B71B-AB769C17B879' ,@PageNumber=1,@RowspPage=5000
ALTER PROCEDURE [dbo].[GS_getRecordsByJobId]
	@JobId uniqueidentifier,@PageNumber  int =1,@RowspPage int =5000 
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
	left JOIN dbo.JobHistory jh
		ON jh.HistoryId=r.HistoryId 
	WHERE R.JobId = @JobId AND 
	(R.Retry < J.MaxRetries OR R.Retry IS NULL) AND
	(R.StatusId = 0 or R.StatusId = 4 OR R.StatusId IS NULL) AND 
	((jh.HistoryId is not null and jh.IsGetDataComplete=1) OR r.HistoryId='00000000-0000-0000-0000-000000000000')
	order by CreatedOn
OFFSET ((@PageNumber - 1) * @RowspPage) ROWS FETCH NEXT @RowspPage ROWS ONLY;
END
