CREATE TABLE [ref].[STRING_VALUE_DISPLAY_MAPPING] (
    [id]            BIGINT         IDENTITY (1, 1) NOT NULL,
    [property_id]   BIGINT         NOT NULL,
    [string_value]  NVARCHAR (512) NOT NULL,
    [display_value] NVARCHAR (512) NOT NULL,
    CONSTRAINT [PK_STRING_VALUE_DISPLAY_MAPPING] PRIMARY KEY CLUSTERED ([id] ASC)
);

