CREATE TABLE [dbo].[VectorSetJson] (
    [VectorSetId]      BIGINT        NOT NULL,
    [VectorSetJsonFileTypeId]  BIGINT        NOT NULL,
    [Content]   VARCHAR (MAX) NOT NULL,
    [CreatedOn] DATETIME      NOT NULL,
    CONSTRAINT [PK_VectorSetJson] PRIMARY KEY CLUSTERED ([VectorSetId] ASC, [VectorSetJsonFileTypeId] ASC),
    CONSTRAINT [FK_VectorSetJson_VectorSets] FOREIGN KEY ([VectorSetId]) REFERENCES [dbo].[VectorSets] ([VectorSetId]),
    CONSTRAINT [FK_VectorSetJson_VectorSetJsonFileTypes] FOREIGN KEY ([VectorSetJsonFileTypeId]) REFERENCES [dbo].[VectorSetJsonFileTypes] ([VectorSetJsonFileTypeId])
);
