CREATE TABLE [external].[SecretKeyValuePairs] (
    [ConfigKey]   VARCHAR (256)   NOT NULL,
    [ConfigValue] VARBINARY (MAX) NOT NULL,
    [UpdatedOn]   DATETIME2 (7)   CONSTRAINT [DF_SecretKeyValuePairs_UpdatedOn] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_SecretKeyValuePairs] PRIMARY KEY CLUSTERED ([ConfigKey] ASC)
);

