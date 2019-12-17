CREATE TABLE [val].[PRODUCT_INFORMATION] (
    [id]                         BIGINT          NOT NULL,
    [vendor_id]                  BIGINT          NOT NULL,
    [address_id]                 BIGINT          NOT NULL,
    [product_url]                NVARCHAR (1024) NULL,
    [module_name]                NVARCHAR (1024) NOT NULL,
    [module_type]                INT             NOT NULL,
    [module_version]             NVARCHAR (128)  NOT NULL,
    [implementation_description] NVARCHAR (MAX)  NOT NULL,
    [itar]                       BIT             NOT NULL
);

