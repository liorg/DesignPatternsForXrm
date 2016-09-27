USE [JobManager]
GO
/****** Object:  StoredProcedure [dbo].[GS_GetLastJobDate]    Script Date: 26/09/2016 15:18:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
-- exec  [dbo].[GS_GetLastJobDate] 'SurveyInterface1.0.0.0'
ALTER PROCEDURE [dbo].[GS_GetLastJobDate]
	@JobName nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;

    SELECT TOP 1 isnull(StartedAt,getdate()) as d 
    FROM dbo.JobHistory H
    INNER JOIN dbo.Jobs J
		ON H.JobId = J.JobId
    WHERE J.JobName = @JobName 
    ORDER BY H.StartedAt desc
END
