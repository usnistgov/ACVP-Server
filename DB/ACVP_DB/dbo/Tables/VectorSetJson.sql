CREATE TABLE [dbo].[VectorSetJson] (
    [VectorSetId]      INT        NOT NULL,
    [VectorSetJsonFileTypeId]  BIGINT        NOT NULL,
    [Content]   VARCHAR (MAX) NOT NULL,
    [CreatedOn] DATETIME      NOT NULL,
    PRIMARY KEY CLUSTERED ([VectorSetId] ASC, [VectorSetJsonFileTypeId] ASC),
    CONSTRAINT [FK_FileType] FOREIGN KEY ([VectorSetJsonFileTypeId]) REFERENCES [dbo].[VectorSetJsonFileTypes] ([VectorSetJsonFileTypeId])
);
