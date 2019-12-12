CREATE TABLE [val].[PRODUCT_INFORMATION] (
    [id]                         BIGINT          IDENTITY (1, 1) NOT NULL,
    [vendor_id]                  BIGINT          NOT NULL,
    [address_id]                 BIGINT          NOT NULL,
    [product_url]                NVARCHAR (1024) NULL,
    [module_name]                NVARCHAR (1024) NOT NULL,
    [module_type]                INT             NOT NULL,
    [module_version]             NVARCHAR (128)  NOT NULL,
    [implementation_description] NVARCHAR (MAX)  NOT NULL,
    [itar]                       BIT             NOT NULL,
    CONSTRAINT [PK_PRODUCT_INFORMATION] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_PRODUCT_INFORMATION_ADDRESS_ID] FOREIGN KEY ([address_id]) REFERENCES [val].[ADDRESS] ([id]),
    CONSTRAINT [FK_PRODUCT_INFORMATION_MODULE_TYPE_ID] FOREIGN KEY ([module_type]) REFERENCES [ref].[MODULE_TYPE] ([id]),
    CONSTRAINT [FK_PRODUCT_INFORMATION_VENDOR_ID] FOREIGN KEY ([vendor_id]) REFERENCES [val].[ORGANIZATION] ([id])
);

