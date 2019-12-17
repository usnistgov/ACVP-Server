CREATE TABLE [val].[VALIDATION_RECORD] (
    [id]                     BIGINT        NOT NULL,
    [product_information_id] BIGINT        NOT NULL,
    [source_id]              INT           NOT NULL,
    [validation_id]          BIGINT        NOT NULL,
    [created_on]             DATETIME2 (7) NOT NULL,
    [updated_on]             DATETIME2 (7) NOT NULL
);

