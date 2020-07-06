CREATE TABLE [dbo].[VectorSetStatuses]
(
	[VectorSetStatusId] INT NOT NULL , 
    [StatusName] VARCHAR(100) NOT NULL,
	CONSTRAINT	[PK_VectorSetStatuses] PRIMARY KEY CLUSTERED ([VectorSetStatusId] ASC)
)
