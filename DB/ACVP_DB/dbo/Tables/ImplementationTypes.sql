CREATE TABLE [dbo].[ImplementationTypes] (
    [ImplementationTypeId]   INT           NOT NULL,
    [ImplementationTypeName] NVARCHAR (10) NOT NULL,
    CONSTRAINT [PK_ImplementationTypes] PRIMARY KEY CLUSTERED ([ImplementationTypeId] ASC),
    CONSTRAINT [UQ_ImplementationTypes] UNIQUE NONCLUSTERED ([ImplementationTypeName] ASC)
);

