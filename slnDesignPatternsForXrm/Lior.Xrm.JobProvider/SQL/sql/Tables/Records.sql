USE [JobManager]
GO

/****** Object:  Table [dbo].[Records]    Script Date: 25/09/2016 17:39:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Records](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[ModelXml] [xml] NULL,
	[StatusId] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedOn] [datetime] NULL,
	[JobId] [uniqueidentifier] NULL,
	[Retry] [int] NULL,
	[ModelType] [nvarchar](100) NULL,
	[Action] [nvarchar](500) NULL,
	[HistoryId] [uniqueidentifier] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[Records] ADD  CONSTRAINT [DF_Records_ID]  DEFAULT (newid()) FOR [ID]
GO


