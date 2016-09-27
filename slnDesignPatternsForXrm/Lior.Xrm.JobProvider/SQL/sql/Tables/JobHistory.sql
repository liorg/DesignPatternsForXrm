USE [JobManager]
GO

/****** Object:  Table [dbo].[JobHistory]    Script Date: 26/09/2016 13:18:43 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[JobHistory](
	[HistoryId] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[StartedAt] [datetime] NULL,
	[EndedAt] [datetime] NULL,
	[TotalRecords] [int] NULL,
	[Failed] [int] NULL,
	[JobId] [uniqueidentifier] NOT NULL,
	[status] [int] NULL,
	[Inserted] [int] NULL,
	[Updated] [int] NULL,
	[NoUpdated] [int] NULL,
	[Succeeded] [int] NULL,
	[IsGetDataComplete] [bit] NOT NULL
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[JobHistory] ADD  CONSTRAINT [DF_JobHistory_HistoryId]  DEFAULT (newid()) FOR [HistoryId]
GO


