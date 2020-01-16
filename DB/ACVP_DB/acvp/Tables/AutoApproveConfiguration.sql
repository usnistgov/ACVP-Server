CREATE TABLE [acvp].[AutoApproveConfiguration] (
    [APIActionID] INT NOT NULL,
    [AutoApprove]     BIT CONSTRAINT [DF_AutoApproveConfiguration_AutoApprove] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_AutoApproveConfiguration] PRIMARY KEY CLUSTERED ([APIActionID] ASC),
    CONSTRAINT [FK_AutoApproveConfiguration_APIActionID] FOREIGN KEY ([APIActionID]) REFERENCES [acvp].[APIActions] ([APIActionID])
);

