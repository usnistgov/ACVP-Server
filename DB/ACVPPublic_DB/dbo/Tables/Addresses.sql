CREATE TABLE [dbo].[Addresses] (
    [AddressId]      BIGINT          IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [OrganizationId] BIGINT          NOT NULL,
    [OrderIndex]     INT             NOT NULL,
    [Street1]        NVARCHAR (1024) NULL,
    [Street2]        NVARCHAR (1024) NULL,
    [Street3]        NVARCHAR (1024) NULL,
    [Locality]       NVARCHAR (1024) NULL,
    [Region]         NVARCHAR (1024) NULL,
    [Country]        NVARCHAR (128)  NULL,
    [PostalCode]     NVARCHAR (128)  NULL,
    CONSTRAINT [PK_Addresses] PRIMARY KEY CLUSTERED ([AddressId] ASC),
    CONSTRAINT [UQ_Addresses] UNIQUE NONCLUSTERED ([OrganizationId] ASC, [OrderIndex] ASC)
);

