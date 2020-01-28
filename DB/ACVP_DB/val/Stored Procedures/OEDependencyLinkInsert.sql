CREATE PROCEDURE [val].[OEDependencyLinkInsert]

	 @OEID bigint
	,@DependencyID bigint

AS

SET NOCOUNT ON

INSERT INTO val.VALIDATION_OE_DEPENDENCY_LINK (validation_oe_id, dependency_id)
VALUES (@OEID, @DependencyID)