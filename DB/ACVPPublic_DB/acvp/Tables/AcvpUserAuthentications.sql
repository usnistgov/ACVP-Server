CREATE TABLE [acvp].[AcvpUserAuthentications] (
    [AcvpUserID]     BIGINT NOT NULL,
    [LastUsedWindow] BIGINT NULL,
    FOREIGN KEY ([AcvpUserID]) REFERENCES [acvp].[ACVP_USER] ([id]), 
    CONSTRAINT [PK_AcvpUserAuthentications] PRIMARY KEY ([AcvpUserID])
);

