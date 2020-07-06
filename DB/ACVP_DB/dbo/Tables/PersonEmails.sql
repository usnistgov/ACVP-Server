CREATE TABLE [dbo].[PersonEmails] (
    [PersonId]     BIGINT        NOT NULL,
    [OrderIndex]   INT           NOT NULL,
    [EmailAddress] VARCHAR (512) NOT NULL,
    CONSTRAINT [PK_PersonEmails] PRIMARY KEY CLUSTERED ([PersonId] ASC, [EmailAddress] ASC),
    CONSTRAINT [FK_PersonEmails_People] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[People] ([PersonId]),
    CONSTRAINT [UQ_PersonEmails] UNIQUE NONCLUSTERED ([PersonId] ASC, [OrderIndex] ASC)
);

