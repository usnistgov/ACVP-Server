CREATE TABLE [ref].[CRYPTO_ALGORITHM_PROPERTY_PROTOCOL] (
    [id]                    BIGINT         IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [property_id]           BIGINT         NOT NULL,
    [capability_field_name] NVARCHAR (128) NULL,
    [vector_field_name]     NVARCHAR (128) NULL,
    [group_field_name]      NVARCHAR (128) NULL,
    [value_field_name]      NVARCHAR (128) NULL,
    [capability_info]       NVARCHAR (512) NOT NULL,
    [vector_info]           NVARCHAR (4)   NOT NULL,
    [group_info]            NVARCHAR (512) NOT NULL,
    [value_info]            NVARCHAR (512) NOT NULL,
    CONSTRAINT [PK_CRYPTO_ALGORITHM_PROPERTY_PROTOCOL] PRIMARY KEY CLUSTERED ([id] ASC)
);

