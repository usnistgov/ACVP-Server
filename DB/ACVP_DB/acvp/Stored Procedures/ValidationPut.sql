CREATE PROCEDURE [acvp].[ValidationPut]
    @VsID INT,
    @Validation NVARCHAR(MAX)
AS
    UPDATE [acvp].[VectorSetJson]
    SET ValidationResults = @Validation
    WHERE VsID = @VsID
GO