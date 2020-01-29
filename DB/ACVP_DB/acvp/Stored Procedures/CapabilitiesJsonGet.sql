-- Grabs the provided vsId from VECTOR_SET_EXPECTED_RESULTS
CREATE PROCEDURE [acvp].[CapabilitiesGet]
    @VsID INT
AS
    SELECT Capabilities
    FROM [acvp].[VectorSetJson] vector_set_table
    WHERE vector_set_table.VsID = @VsID

GO
