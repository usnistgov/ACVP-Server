CREATE PROCEDURE [dbo].[ImplementationGet]

    @ImplementationId BIGINT

AS

SET NOCOUNT ON

SELECT ImplementationId
    ,OrganizationId
    ,AddressId
    ,[Url]
    ,ImplementationName
    ,ImplementationTypeId
    ,ImplementationVersion
    ,ImplementationDescription
FROM dbo.Implementations
WHERE ImplementationId = @ImplementationId
  AND ITAR = 0