USE [JobManager]
GO

/****** Object:  Table [dbo].[Records]    Script Date: 31/10/2016 18:26:10 ******/
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
	[HistoryId] [uniqueidentifier] NOT NULL,
	[StatusCode] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[Records] ADD  CONSTRAINT [DF_Records_ID]  DEFAULT (newid()) FOR [ID]
GO


