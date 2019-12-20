CREATE TABLE [val].[PERSON_NOTE] (
    [id]           BIGINT          IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [person_id]    BIGINT          NOT NULL,
    [note_date]    DATETIME2 (7)   NOT NULL,
    [note_subject] NVARCHAR (1024) NOT NULL,
    [note]         NVARCHAR (MAX)  NOT NULL,
    CONSTRAINT [PK_PERSON_NOTE] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_PERSON_NOTE_PERSON_ID] FOREIGN KEY ([person_id]) REFERENCES [val].[PERSON] ([id])
);

