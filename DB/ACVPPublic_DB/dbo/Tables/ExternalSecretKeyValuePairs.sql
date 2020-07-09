CREATE TABLE [dbo].[ExternalSecretKeyValuePairs] (
    [ConfigKey]   VARCHAR (256)   NOT NULL,
    [ConfigValue] VARBINARY (MAX) NOT NULL,
    [UpdatedOn]   DATETIME2 (7)   CONSTRAINT [DF_ExternalSecretKeyValuePairs_UpdatedOn] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_ExternalSecretKeyValuePairs] PRIMARY KEY CLUSTERED ([ConfigKey] ASC)
);

