CREATE PROCEDURE [dbo].[AddressInsert]
	 
	 @OrganizationId bigint
	,@OrderIndex int
	,@Street1 nvarchar(1024)
	,@Street2 nvarchar(1024)
	,@Street3 nvarchar(1024)
	,@Locality nvarchar(1024)
	,@Region nvarchar(1024)
	,@Country nvarchar(128)
	,@PostalCode nvarchar(128)

AS

SET NOCOUNT ON

INSERT INTO dbo.Addresses (
	 OrganizationId
	,OrderIndex
	,Street1
	,Street2
	,Street3
	,Locality
	,Region
	,Country
	,PostalCode
)
VALUES (
	 @OrganizationId
	,@OrderIndex
	,@Street1
	,@Street2
	,@Street3
	,@Locality
	,@Region
	,@Country
	,@PostalCode
)

SELECT CAST(SCOPE_IDENTITY() AS bigint) AS AddressId
