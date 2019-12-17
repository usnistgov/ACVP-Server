CREATE TABLE [ref].[CRYPTO_ALGORITHM_PROPERTY] (
    [id]               BIGINT         IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [algorithm_id]     BIGINT         NOT NULL,
    [name]             NVARCHAR (128) NOT NULL,
    [capability_type]  NCHAR (4)      NULL,
    [vector_type]      NCHAR (4)      NULL,
    [group_type]       NCHAR (4)      NULL,
    [value_type]       NCHAR (4)      NULL,
    [default_value]    NVARCHAR (128) NULL,
    [in_certificate]   BIT            NOT NULL,
    [certificate_name] NVARCHAR (128) NULL,
    [order_index]      INT            NULL,
    [historical]       BIT            NULL,
    [required]         BIT            NULL,
    [units_label]      NVARCHAR (128) NULL,
    CONSTRAINT [PK_CRYPTO_ALGORITHM_PROPERTY] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [UQ_CRYPTO_ALGORITHM_PROPERTY_NAME] UNIQUE NONCLUSTERED ([algorithm_id] ASC, [name] ASC)
);

