USE [JobManager]
GO
/****** Object:  StoredProcedure [dbo].[GS_InsertJobHistory]    Script Date: 22/09/2016 15:21:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[GS_InsertJobHistory]
@jobid uniqueidentifier,@StartedAt datetime=null
as
begin
declare @op table
(
    ColGuid uniqueidentifier
)

INSERT INTO dbo.JobHistory
           (StartedAt,IsGetDataComplete    , JobId)
   OUTPUT INSERTED.HistoryId  into @op
     VALUES
           (ISNULL(@StartedAt, getdate()),0 ,
           @jobid)


select o.ColGuid from @op o

end
