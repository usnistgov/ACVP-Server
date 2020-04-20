CREATE TABLE [acvp].[Request] (
    [ID]          BIGINT        NOT NULL,
    [APIActionID] INT           NOT NULL,
    [UserID]      BIGINT        NOT NULL,
    [CreatedOn]   DATETIME2 (7) NOT NULL,
    [Status]      INT           NOT NULL,
    [AcceptID]    BIGINT        NULL
);

