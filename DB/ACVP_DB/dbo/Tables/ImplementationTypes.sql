CREATE TABLE [dbo].[ImplementationTypes] (
    [ImplementationTypeId]   INT           NOT NULL,
    [ImplementationTypeName] NVARCHAR (10) NOT NULL,
    CONSTRAINT [PK_MODULE_TYPE] PRIMARY KEY CLUSTERED ([ImplementationTypeId] ASC),
    CONSTRAINT [UQ_MODULE_TYPE_NAME] UNIQUE NONCLUSTERED ([ImplementationTypeName] ASC)
);

