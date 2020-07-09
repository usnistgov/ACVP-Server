CREATE TABLE [dbo].[VectorSetJsonFileTypes] (
    [VectorSetJsonFileTypeId] BIGINT       NOT NULL,
    [FileType]                VARCHAR (20) NOT NULL,
    CONSTRAINT [PK_VectorSetJsonFileTypes] PRIMARY KEY CLUSTERED ([VectorSetJsonFileTypeId] ASC)
);

