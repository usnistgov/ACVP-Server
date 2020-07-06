CREATE TABLE [dbo].[Implementations] (
    [ImplementationId]                         BIGINT          IDENTITY (1, 1) NOT NULL,
    [OrganizationId]                  BIGINT          NOT NULL,
    [AddressId]                 BIGINT          NOT NULL,
    [Url]                NVARCHAR (1024) NULL,
    [ImplementationName]                NVARCHAR (1024) NOT NULL,
    [ImplementationTypeId]                INT             NOT NULL,
    [ImplementationVersion]             NVARCHAR (128)  NOT NULL,
    [ImplementationDescription] NVARCHAR (MAX)  NOT NULL,
    [ITAR]                       BIT             NOT NULL,
    CONSTRAINT [PK_Implementations] PRIMARY KEY CLUSTERED ([ImplementationId] ASC),
    CONSTRAINT [FK_Implementations_Addresses] FOREIGN KEY ([AddressId]) REFERENCES [dbo].[Addresses] ([AddressId]),
    CONSTRAINT [FK_Implementations_ImplementationTypes] FOREIGN KEY ([ImplementationTypeId]) REFERENCES [dbo].[ImplementationTypes] ([ImplementationTypeId]),
    CONSTRAINT [FK_Implementations_Organizations] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organizations] ([OrganizationId])
);

