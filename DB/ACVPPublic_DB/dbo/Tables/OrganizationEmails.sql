CREATE TABLE [dbo].[OrganizationEmails] (
    [OrganizationId] BIGINT        NOT NULL,
    [OrderIndex]     INT           NOT NULL,
    [EmailAddress]   VARCHAR (512) NOT NULL,
    CONSTRAINT [PK_OrganizationEmails] PRIMARY KEY CLUSTERED ([OrganizationId] ASC, [EmailAddress] ASC),
    CONSTRAINT [UQ_OrganizationEmails] UNIQUE NONCLUSTERED ([OrganizationId] ASC, [OrderIndex] ASC)
);

