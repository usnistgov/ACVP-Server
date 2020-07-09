CREATE TABLE [dbo].[VectorSetJson] (
    [VectorSetId]             BIGINT        NOT NULL,
    [VectorSetJsonFileTypeId] BIGINT        NOT NULL,
    [Content]                 VARCHAR (MAX) NOT NULL,
    [CreatedOn]               DATETIME      NOT NULL
);


GO
CREATE UNIQUE CLUSTERED INDEX [PK_FilteredVectorSetJson]
    ON [dbo].[VectorSetJson]([VectorSetId] ASC, [VectorSetJsonFileTypeId] ASC);

