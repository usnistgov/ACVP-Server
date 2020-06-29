CREATE TABLE [dbo].[Implementations] (
    [ImplementationId]                         BIGINT          IDENTITY (1, 1) NOT NULL,
    [VendorId]                  BIGINT          NOT NULL,
    [AddressId]                 BIGINT          NOT NULL,
    [Url]                NVARCHAR (1024) NULL,
    [ImplementationName]                NVARCHAR (1024) NOT NULL,
    [ImplementationTypeId]                INT             NOT NULL,
    [ImplementationVersion]             NVARCHAR (128)  NOT NULL,
    [ImplementationDescription] NVARCHAR (MAX)  NOT NULL,
    [ITAR]                       BIT             NOT NULL,
    CONSTRAINT [PK_Implementations] PRIMARY KEY CLUSTERED ([ImplementationId] ASC),
    CONSTRAINT [FK_Implementations_AddressId] FOREIGN KEY ([AddressId]) REFERENCES [dbo].[Addresses] ([AddressId]),
    CONSTRAINT [FK_Implementations_ImplementationTypeId] FOREIGN KEY ([ImplementationTypeId]) REFERENCES [dbo].[ImplementationTypes] ([ImplementationTypeId]),
    CONSTRAINT [FK_Implementations_VendorId] FOREIGN KEY ([VendorId]) REFERENCES [dbo].[Organizations] ([OrganizationId])
);

