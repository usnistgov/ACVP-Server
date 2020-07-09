CREATE PROCEDURE [dbo].[DependencyAttributeGet]

    @DependencyId BIGINT
	
AS

SELECT   [Name]
        ,[Value]
FROM dbo.DependencyAttributes
WHERE DependencyId = @DependencyId