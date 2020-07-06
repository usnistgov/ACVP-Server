CREATE TABLE [dbo].[ACVPUsers] (
    [ACVPUserId]          BIGINT          IDENTITY (1, 1) NOT NULL,
    [PersonId]   BIGINT          NULL,
    [CommonName] NVARCHAR (2048) NOT NULL,
    [Certificate] VARBINARY (MAX) NULL,
    [Seed]        NVARCHAR (64)   NOT NULL,
    [ExpiresOn] DATETIME2 NULL, 
    CONSTRAINT [PK_ACVPUsers] PRIMARY KEY CLUSTERED ([ACVPUserId] ASC),
    CONSTRAINT [FK_ACVPUsers_People] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[People] ([PersonId])
);

