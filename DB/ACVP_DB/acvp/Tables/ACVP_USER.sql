CREATE TABLE [acvp].[ACVP_USER] (
    [id]          BIGINT          IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [person_id]   BIGINT          NULL,
    [common_name] NVARCHAR (2048) NOT NULL,
    [certificate] VARBINARY (MAX) NULL,
    [seed]        NVARCHAR (64)   NOT NULL,
    [expiresOn] DATETIME2 NULL, 
    CONSTRAINT [PK_ACVP_USER] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_ACVP_USER_PERSON_ID] FOREIGN KEY ([person_id]) REFERENCES [val].[PERSON] ([id])
);

