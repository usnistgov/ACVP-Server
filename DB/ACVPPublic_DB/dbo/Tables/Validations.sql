CREATE TABLE [dbo].[Validations] (
    [ValidationId]       BIGINT        NOT NULL,
    [ImplementationId]   BIGINT        NOT NULL,
    [ValidationSourceId] INT           NOT NULL,
    [ValidationNumber]   BIGINT        NOT NULL,
    [CreatedOn]          DATETIME2 (7) NOT NULL,
    [LastUpdated]        DATETIME2 (7) NOT NULL
);


GO
CREATE UNIQUE CLUSTERED INDEX [PK_CSRC_Validations]
    ON [dbo].[Validations]([ValidationId] ASC);

