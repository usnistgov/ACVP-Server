CREATE TABLE [dbo].[Algorithms] (
    [AlgorithmId]   BIGINT         NOT NULL,
    [Name] NVARCHAR (128) NOT NULL,
    [Mode]          NVARCHAR (128) NULL,
    [Revision]      NVARCHAR (32)  NULL,
    [Alias]         NVARCHAR (128) NULL,
    [DisplayName]   NVARCHAR (128) NULL,
    [Historical]    BIT            CONSTRAINT [DF_Algorithms_Historical] DEFAULT ((0)) NOT NULL,
    [DisplayOnCSRC] BIT            CONSTRAINT [DF_Algorithms_DisplayOnCSRC] DEFAULT ((0)) NOT NULL,
    [SupportedByACVP] BIT          CONSTRAINT [DF_Algorithms_SupportedByACVP] DEFAULT ((0)) NOT NULL,
    [PrimitiveId]   BIGINT         NOT NULL,
    CONSTRAINT [PK_Algorithms] PRIMARY KEY CLUSTERED ([AlgorithmId] ASC),
    CONSTRAINT [UQ_Algorithms_Name_Mode_Revision] UNIQUE NONCLUSTERED ([Name] ASC, [Mode] ASC, [Revision] ASC)
);

