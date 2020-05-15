CREATE TABLE [acvp].[Request] (
    [RequestID]  BIGINT        NOT NULL,
    [UserID]     BIGINT        NOT NULL,
    [Created]    DATETIME2 (7) NOT NULL,
    [Status]     INT           NOT NULL,
    [ApprovedID] BIGINT        NULL,
    [APIAction]  INT           NULL
);



