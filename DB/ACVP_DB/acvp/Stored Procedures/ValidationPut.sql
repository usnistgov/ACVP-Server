SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [acvp].[ValidationPut]
    @VsID INT,
    @Validation NVARCHAR(MAX)
AS
    UPDATE [acvp].[VectorSetJson]
    SET ValidationResults = @Validation
    WHERE VsID = @VsID
GO