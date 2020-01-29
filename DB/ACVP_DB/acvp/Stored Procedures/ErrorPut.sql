CREATE PROCEDURE [acvp].[ErrorPut]
    @VsID INT,
    @Error NVARCHAR(MAX)
AS
    UPDATE [acvp].[VectorSetJson]
    SET Error = @Error
    WHERE VsID = @VsID
GO