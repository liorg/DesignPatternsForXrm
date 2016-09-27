USE [JobManager]
GO

/****** Object:  StoredProcedure [dbo].[GS_getFaildRecords]    Script Date: 21/09/2016 14:16:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================


CREATE PROCEDURE [dbo].[GS_getFaildRecords]
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
			into #t
	FROM dbo.Records R
	INNER JOIN dbo.Jobs J
		ON R.JobId=J.JobId
	WHERE R.JobId = @JobId 
	AND R.Retry = J.MaxRetries 
	AND R.StatusId = 4 --Failed
	
	update RD 
	SET RD.Retry=#t.Retry+1
	FROM dbo.Records RD
	inner join #t
	on #t.ID=RD.ID
	
	
	select * from #t
	
END

GO


