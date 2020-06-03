CREATE TABLE [dbo].[Validations]
(
	[ValidationId] BIGINT IDENTITY(1,1) NOT NULL  PRIMARY KEY, 
    [ImplementationId] BIGINT NOT NULL, 
    [ValidationSourceId] INT NOT NULL, 
    [ValidationNumber] BIGINT NOT NULL, 
    [CreatedOn] DATETIME2 NOT NULL, 
    [LastUpdated] DATETIME2 NOT NULL
)
