CREATE TABLE [dbo].[ACVPUserAuthentications]
(
	[ACVPUserId]     BIGINT NOT NULL,
    [LastUsedWindow] BIGINT NULL,
    CONSTRAINT [PK_ACVPUserAuthentications] PRIMARY KEY ([ACVPUserId]),
    CONSTRAINT [FK_ACVPUserAuthentications_ACVPUsers] FOREIGN KEY ([ACVPUserId]) REFERENCES [dbo].[ACVPUsers] ([ACVPUserId])

)
