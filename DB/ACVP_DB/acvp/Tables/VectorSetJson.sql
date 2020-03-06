CREATE TABLE [acvp].[VectorSetJson] (
    [VsId]      INT        NOT NULL,
    [FileType]  BIGINT        NOT NULL,
    [Content]   VARCHAR (MAX) NOT NULL,
    [CreatedOn] DATETIME      NOT NULL,
    PRIMARY KEY CLUSTERED ([VsId] ASC, [FileType] ASC),
    CONSTRAINT [FK_FileType] FOREIGN KEY ([FileType]) REFERENCES [common].[JsonFileType] ([Id])
);
