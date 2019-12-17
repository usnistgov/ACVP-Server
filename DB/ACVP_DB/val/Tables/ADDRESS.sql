CREATE TABLE [val].[ADDRESS] (
    [id]                  BIGINT          IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [organization_id]     BIGINT          NOT NULL,
    [order_index]         INT             NOT NULL,
    [address_street1]     NVARCHAR (1024) NULL,
    [address_street2]     NVARCHAR (1024) NULL,
    [address_street3]     NVARCHAR (1024) NULL,
    [address_locality]    NVARCHAR (1024) NULL,
    [address_region]      NVARCHAR (1024) NULL,
    [address_country]     NVARCHAR (128)  NULL,
    [address_postal_code] NVARCHAR (128)  NULL,
    CONSTRAINT [PK_ADDRESS] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_ADDRESS_ORGANIZATION_ID] FOREIGN KEY ([organization_id]) REFERENCES [val].[ORGANIZATION] ([id]),
    CONSTRAINT [UQ_ADDRESS] UNIQUE NONCLUSTERED ([organization_id] ASC, [order_index] ASC)
);

