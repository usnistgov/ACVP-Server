-- Grabs the provided vsId from VECTOR_SET_EXPECTED_RESULTS
CREATE PROCEDURE [acvp].[SubmittedGet]
    @VsID INT
AS
    SELECT SubmittedResults, InternalProjection
    FROM [acvp].[VectorSetJson] vector_set_table
    WHERE vector_set_table.VsID = @VsID

GO
