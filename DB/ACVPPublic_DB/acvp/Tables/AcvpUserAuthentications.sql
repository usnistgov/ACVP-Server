CREATE TABLE [acvp].[AcvpUserAuthentications] (
    [AcvpUserID]     BIGINT NULL,
    [LastUsedWindow] BIGINT NULL,
    FOREIGN KEY ([AcvpUserID]) REFERENCES [acvp].[ACVP_USER] ([id])
);

