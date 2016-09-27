USE [JobManager]
GO
/****** Object:  StoredProcedure [dbo].[GS_UpdateRecord]    Script Date: 22/09/2016 10:17:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[GS_UpdateRecord]
	-- Add the parameters for the stored procedure here
@status int,@recordid uniqueidentifier, @retry int=null
,@action nvarchar(200)=null, @ModelXml xml=null,@isDelSuccess bit=0
AS
BEGIN
	SET NOCOUNT ON;
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	if(@isDelSuccess=1 AND @status=3 )
	begin
			DELETE FROM [dbo].[Records] WHERE [ID] = @recordid
	end
	else
	begin
			UPDATE [dbo].[Records]
			SET [StatusId] =@status
			  ,[ModifiedOn] = getdate()
			  ,[Retry] = ISNULL(@retry,[Retry]) 
			  ,[Action] = ISNULL(@action,[Action])
			  ,[modelxml]= ISNULL(@ModelXml,[modelxml])
			WHERE [ID] = @recordid
	end

END
