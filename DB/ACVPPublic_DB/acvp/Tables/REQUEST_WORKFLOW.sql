CREATE TABLE [acvp].[REQUEST_WORKFLOW] (
    [id]         BIGINT        NOT NULL,
    [action_id]  INT           NOT NULL,
    [user_id]    BIGINT        NOT NULL,
    [created_on] DATETIME2 (7) NOT NULL,
    [type]       INT           NOT NULL,
    [status]     INT           NOT NULL,
    [accept_id]  BIGINT        NULL
);

