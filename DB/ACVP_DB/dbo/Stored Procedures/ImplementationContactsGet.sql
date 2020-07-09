CREATE PROCEDURE [dbo].[ImplementationContactsGet]
	
	@ImplementationId bigint

AS

SET NOCOUNT ON

SELECT	 P.PersonId
		,P.FullName
FROM dbo.ImplementationContacts IC
	INNER JOIN
	dbo.People P ON P.PersonId = IC.PersonId
				AND IC.ImplementationId = @ImplementationId
ORDER BY IC.OrderIndex