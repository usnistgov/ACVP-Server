﻿CREATE PROCEDURE [val].[NextACVPValidationNumberGet]

AS

SET NOCOUNT ON

INSERT INTO val.ACVP_ID_GENERATOR DEFAULT VALUES

SELECT CAST(SCOPE_IDENTITY() AS bigint) AS ValidationNumber