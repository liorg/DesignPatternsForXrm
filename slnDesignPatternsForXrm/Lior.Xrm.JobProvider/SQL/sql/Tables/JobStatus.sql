USE [JobManager]
GO

/****** Object:  Table [dbo].[JobStatus]    Script Date: 21/09/2016 14:14:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[JobStatus](
	[StatusId] [int] NOT NULL,
	[StatusName] [nvarchar](50) NULL
) ON [PRIMARY]

GO


