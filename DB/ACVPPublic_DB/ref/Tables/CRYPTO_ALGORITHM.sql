CREATE TABLE [ref].[CRYPTO_ALGORITHM] (
    [id]           BIGINT         NOT NULL,
    [name]         NVARCHAR (128) NOT NULL,
    [primitive_id] BIGINT         NOT NULL,
    [mode]         NVARCHAR (128) NULL,
    [revision]     NVARCHAR (32)  NULL,
    [alias]        NVARCHAR (128) NULL,
    [display_name] NVARCHAR (128) NULL,
    [historical]   BIT            NOT NULL,
    CONSTRAINT [PK_CRYPTO_ALGORITHM] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [UQ_CRYPTO_ALGORITHM_NAME_MODE_REVISION] UNIQUE NONCLUSTERED ([name] ASC, [mode] ASC, [revision] ASC)
);

