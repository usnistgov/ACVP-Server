CREATE PROCEDURE [acvp].[AutoApproveConfigurationGet]

AS

SET NOCOUNT ON

SELECT APIActionID, AutoApprove
FROM acvp.AutoApproveConfiguration
