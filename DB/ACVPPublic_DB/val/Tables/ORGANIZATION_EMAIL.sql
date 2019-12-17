CREATE TABLE [val].[ORGANIZATION_EMAIL] (
    [organization_id] BIGINT        NOT NULL,
    [order_index]     INT           NOT NULL,
    [email_address]   VARCHAR (512) NOT NULL,
    CONSTRAINT [PK_ORGANIZATION_EMAIL] PRIMARY KEY CLUSTERED ([organization_id] ASC, [email_address] ASC),
    CONSTRAINT [UQ_ORGANIZATION_EMAIL] UNIQUE NONCLUSTERED ([organization_id] ASC, [order_index] ASC)
);

