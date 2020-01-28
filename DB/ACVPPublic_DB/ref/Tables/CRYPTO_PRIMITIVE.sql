CREATE TABLE [ref].[CRYPTO_PRIMITIVE] (
    [id]          BIGINT         NOT NULL,
    [family_id]   BIGINT         NOT NULL,
    [name]        NVARCHAR (128) NOT NULL,
    [header_text] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_CRYPTO_PRIMITIVE] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [UQ_CRYPTO_PRIMITIVE] UNIQUE NONCLUSTERED ([family_id] ASC, [name] ASC)
);

