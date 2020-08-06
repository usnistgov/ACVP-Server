CREATE TABLE [dbo].[Dependencies] (
    [DependencyId]   BIGINT          IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [DependencyType] NVARCHAR (1024) NOT NULL,
    [Name]           NVARCHAR (1024) NOT NULL,
    [Description]    NVARCHAR (2048) NULL,
    [EffectiveITAR]  BIT             CONSTRAINT [DF_Dependencies_EffectiveITAR] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Dependencies] PRIMARY KEY CLUSTERED ([DependencyId] ASC)
);



