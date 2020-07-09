CREATE PROCEDURE [dbo].[DependencyGet]

    @DependencyId BIGINT
	
AS

SELECT   DependencyId
        ,DependencyType
        ,[Name]
        ,[Description]
FROM dbo.Dependencies
WHERE DependencyId = @DependencyId