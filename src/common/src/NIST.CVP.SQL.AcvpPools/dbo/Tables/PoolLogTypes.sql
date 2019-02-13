CREATE TABLE [dbo].[PoolLogTypes] (
    [id]                 BIGINT        IDENTITY (1, 1) NOT NULL,
    [logTypeDescription] VARCHAR (256) NOT NULL,
    CONSTRAINT [PK_PoolLogTypes] PRIMARY KEY CLUSTERED ([id] ASC)
);

