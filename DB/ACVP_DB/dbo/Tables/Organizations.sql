CREATE TABLE [dbo].[Organizations] (
    [OrganizationId]                     BIGINT          IDENTITY (1, 1) NOT NULL,
    [ParentOrganizationId] BIGINT          NULL,
    [OrganizationName]                   NVARCHAR (1024) NOT NULL,
    [OrganizationUrl]       NVARCHAR (1024) NULL,
    [VoiceNumber]           NVARCHAR (64)   NULL,
    [FaxNumber]             NVARCHAR (64)   NULL,
    CONSTRAINT [PK_Organizations] PRIMARY KEY CLUSTERED ([OrganizationId] ASC),
    CONSTRAINT [FK_Organizations_ParentOrganizationId] FOREIGN KEY ([ParentOrganizationId]) REFERENCES [dbo].[Organizations] ([OrganizationId])
);

