CREATE TABLE [val].[PERSON] (
    [id]        BIGINT          IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [full_name] NVARCHAR (1024) NOT NULL,
    [org_id]    BIGINT          NULL,
    CONSTRAINT [PK_PERSON] PRIMARY KEY CLUSTERED ([id] ASC)
);

