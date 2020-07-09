CREATE TABLE [dbo].[ACVPUsers] (
    [ACVPUserId]  BIGINT          IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [PersonId]    BIGINT          NULL,
    [CommonName]  NVARCHAR (2048) NOT NULL,
    [Certificate] VARBINARY (MAX) NULL,
    [Seed]        NVARCHAR (64)   NOT NULL,
    [ExpiresOn]   DATETIME2 (7)   NULL,
    CONSTRAINT [PK_ACVPUsers] PRIMARY KEY CLUSTERED ([ACVPUserId] ASC)
);

