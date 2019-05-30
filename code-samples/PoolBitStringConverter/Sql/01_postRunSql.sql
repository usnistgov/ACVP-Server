/*
	This sql script cleans up indexes and columns used in the pool serialization update process.
*/

USE AcvpPools
GO

IF EXISTS (
	SELECT * 
	FROM sys.indexes 
	WHERE name='NonClusteredIndex-PoolId-hasNewlySerializedBitStrings' AND object_id = OBJECT_ID('dbo.PoolValues')
)
BEGIN
	DROP INDEX [NonClusteredIndex-PoolId-hasNewlySerializedBitStrings] ON [dbo].[PoolValues]
END 

GO

IF EXISTS (
	SELECT 1 FROM sys.columns 
    WHERE Name = N'hasNewlySerializedBitStrings'
    AND Object_ID = Object_ID(N'dbo.poolValues')
)

BEGIN
	ALTER TABLE [dbo].[PoolValues] DROP COLUMN [hasNewlySerializedBitStrings]
END 

GO

