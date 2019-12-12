CREATE TABLE [acvp].[ACVP_USER] (
    [id]          BIGINT          IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [person_id]   BIGINT          NULL,
    [common_name] NVARCHAR (2048) NOT NULL,
    [certificate] VARBINARY (MAX) NULL,
    [seed]        NVARCHAR (64)   NOT NULL,
    CONSTRAINT [PK_ACVP_USER] PRIMARY KEY CLUSTERED ([id] ASC)
);

