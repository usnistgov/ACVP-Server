CREATE TABLE [lcavp].[SubmissionRegistrations] (
    [SubmissionRegistrationID] INT            IDENTITY (1, 1) NOT NULL,
    [SubmissionID]             INT            NOT NULL,
    [Status]                   TINYINT        NULL,
    [RegistrationJson]         NVARCHAR (MAX) NULL,
    [ErrorJson]                NVARCHAR (MAX) NULL,
    [WorkflowTypeID]           INT            NULL,
    [WorkflowID]               BIGINT         NULL
);

