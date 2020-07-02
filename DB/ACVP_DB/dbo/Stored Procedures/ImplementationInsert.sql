CREATE PROCEDURE [dbo].[ImplementationInsert]

	 @Name nvarchar(1024)
	,@Description nvarchar(MAX)
	,@ImplementationTypeId int
	,@Version nvarchar(128)
	,@Website nvarchar(1024)
	,@OrganizationId bigint
	,@AddressId bigint
	,@IsITAR bit

AS

SET NOCOUNT ON

INSERT INTO dbo.Implementations (
	 OrganizationId
	,AddressId
	,Url
	,ImplementationName
	,ImplementationTypeId
	,ImplementationVersion
	,ImplementationDescription
	,ITAR
)
VALUES (
	 @OrganizationId
	,@AddressId
	,@Website
	,@Name
	,@ImplementationTypeId
	,@Version
	,@Description
	,@IsITAR
)

SELECT CAST(SCOPE_IDENTITY() AS bigint) AS ImplementationId

