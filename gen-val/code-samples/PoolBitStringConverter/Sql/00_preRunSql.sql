/*
	This sql should be run prior to the running of the console application.
	It adds a new column to track progress through the serialization, as well as an index to assist with queries.

	Once the console application has been run, the secondary cleanup script should be run.
*/

USE AcvpPools
GO

IF NOT EXISTS (
	SELECT 1 FROM sys.columns 
    WHERE Name = N'hasNewlySerializedBitStrings'
    AND Object_ID = Object_ID(N'dbo.poolValues')
)

BEGIN
	ALTER TABLE [dbo].[poolValues]
	ADD hasNewlySerializedBitStrings BIT NOT NULL CONSTRAINT DF_PoolValues_hasNewlySerializedBitStrings DEFAULT(0)
END 

GO

IF NOT EXISTS (
	SELECT * 
	FROM sys.indexes 
	WHERE name='NonClusteredIndex-PoolId-hasNewlySerializedBitStrings' AND object_id = OBJECT_ID('dbo.PoolValues')
)
BEGIN
	CREATE NONCLUSTERED INDEX [NonClusteredIndex-PoolId-hasNewlySerializedBitStrings] ON [dbo].[PoolValues]
	(
		[poolId] ASC,
		[hasNewlySerializedBitStrings] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
END 

GO

