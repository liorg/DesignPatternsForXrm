USE [JobManager]
GO
/****** Object:  StoredProcedure [dbo].[GS_GetLastJobDate]    Script Date: 29/09/2016 16:47:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
-- exec  [dbo].[GS_GetLastJobDate] 'Malam.Crm.Talcar.ActiveTrail.EventCallback1.0.0.0'
ALTER PROCEDURE [dbo].[GS_GetLastJobDate]
	@JobName nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;

    SELECT TOP 1 isnull(StartedAt,getdate()) as d 
    FROM dbo.JobHistory H
    INNER JOIN dbo.Jobs J
		ON H.JobId = J.JobId
    WHERE J.JobName = @JobName and h.IsGetDataComplete=1
    ORDER BY H.StartedAt desc
END
