CREATE PROCEDURE [acvp].[PromptDataPut]
    @VsID INT,
    @Prompt NVARCHAR(MAX),
    @InternalProjection NVARCHAR(MAX),
    @ExpectedResults NVARCHAR(MAX)
AS
    UPDATE [acvp].[VectorSetJson]
    SET Prompt = @Prompt, InternalProjection = @InternalProjection, ExpectedResults = @ExpectedResults
    WHERE VsID = @VsID
GO
