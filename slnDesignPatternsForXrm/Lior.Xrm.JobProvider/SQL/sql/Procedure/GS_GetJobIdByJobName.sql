USE [JobManager]
GO

/****** Object:  StoredProcedure [dbo].[GS_GetJobIdByJobName]    Script Date: 21/09/2016 14:16:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GS_GetJobIdByJobName]
	-- Add the parameters for the stored procedure here
	@jobName nvarchar(100)
AS
BEGIN
SELECT top 1 [JobId]
  FROM [JobManager].[dbo].[Jobs]
where [JobName]=@jobName



END

GO


