CREATE PROCEDURE [dbo].[OEDependenciesDetailsGet]

	@OEId int = 0

AS

	SET NOCOUNT ON

	SELECT	 D.DependencyId
			,D.[Name]
			,D.DependencyType
			,D.[Description]
	FROM dbo.OEDependencies OED
		INNER JOIN
		dbo.Dependencies D ON D.DependencyId = OED.DependencyId
						  AND OED.OEId = @OEId