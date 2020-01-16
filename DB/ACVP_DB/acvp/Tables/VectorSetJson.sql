SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [acvp].[VectorSetJson](
	[VsID] [int] NOT NULL,
	[Capabilities] [varchar](max) NOT NULL,
	[Prompt] [varchar](max) NULL,
	[ExpectedResults] [varchar](max) NULL,
	[SubmittedResults] [varchar](max) NULL,
	[ValidationResults] [varchar](max) NULL,
	[InternalProjection] [varchar](max) NULL,
    [Error] [varchar](max) NULL,
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

