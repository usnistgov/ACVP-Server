CREATE TABLE [ref].[CRYPTO_ALGORITHM_LINK] (
    [algorithm_id]   BIGINT NOT NULL,
    [acv_version_id] INT    NOT NULL,
    CONSTRAINT [PK_CRYPTO_ALGORITHM_LINK] PRIMARY KEY CLUSTERED ([algorithm_id] ASC, [acv_version_id] ASC)
);

