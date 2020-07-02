CREATE PROCEDURE [dbo].[ImplementationsGet]

	@PageSize BIGINT,
	@PageNumber BIGINT,
	@ImplementationId BIGINT = NULL,
	@Name NVARCHAR(1024) = NULL,
	@Description NVARCHAR(MAX) = NULL,
	@TotalRecords BIGINT OUTPUT

AS

	SET NOCOUNT ON

	SELECT  @TotalRecords = COUNT_BIG(1)
	FROM dbo.Implementations I
	WHERE	(@ImplementationId IS NULL OR I.ImplementationId = @ImplementationId)
		AND (@Name IS NULL OR I.ImplementationName LIKE '%' + @Name + '%')
		AND (@Description IS NULL OR I.ImplementationDescription LIKE '%' + @Description + '%')

	SELECT	 I.ImplementationId 
			,I.[Url]
			,I.ImplementationName
			,I.ImplementationVersion
			,I.ImplementationDescription
			,I.ITAR
			,I.ImplementationTypeId
			,O.OrganizationId
			,O.OrganizationName
			,O.OrganizationUrl
			,O.FaxNumber
			,O.VoiceNumber
			,O.ParentOrganizationId
			,A.AddressId
			,A.Street1
			,A.Street2
			,A.Street3
			,A.Locality
			,A.Region
			,A.PostalCode
			,A.Country
	FROM dbo.Implementations I
		INNER JOIN
		dbo.Organizations O ON O.OrganizationId = I.OrganizationId
		INNER JOIN
		dbo.Addresses A ON A.AddressId = I.AddressId
	WHERE	(@ImplementationId IS NULL OR I.ImplementationId = @ImplementationId)
		AND (@Name IS NULL OR I.ImplementationName LIKE '%' + @Name + '%')
		AND (@Description IS NULL OR I.ImplementationDescription LIKE '%' + @Description + '%')
	ORDER BY I.ImplementationId
	OFFSET @PageSize * (@PageNumber - 1) ROWS
	FETCH NEXT @PageSize ROWS ONLY;