CREATE TABLE [dbo].[Validations] (
    [ValidationId]       BIGINT        IDENTITY (1, 1) NOT NULL,
    [ImplementationId]   BIGINT        NOT NULL,
    [ValidationSourceId] INT           NOT NULL,
    [ValidationNumber]   BIGINT        NOT NULL,
    [CreatedOn]          DATETIME2 (7) NOT NULL,
    [LastUpdated]        DATETIME2 (7) NOT NULL,
    CONSTRAINT [PK__Validations] PRIMARY KEY CLUSTERED ([ValidationId] ASC)
);


