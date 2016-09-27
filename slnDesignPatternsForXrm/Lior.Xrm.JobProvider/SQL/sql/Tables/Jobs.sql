USE [JobManager]
GO

/****** Object:  Table [dbo].[Jobs]    Script Date: 21/09/2016 14:13:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Jobs](
	[JobId] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[JobName] [nvarchar](50) NULL,
	[MaxRetries] [int] NULL
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Jobs] ADD  CONSTRAINT [DF_Jobs_JobId]  DEFAULT (newid()) FOR [JobId]
GO


