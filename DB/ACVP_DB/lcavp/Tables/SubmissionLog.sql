CREATE TABLE [lcavp].[SubmissionLog] (
    [SubmissionLogID]       INT            IDENTITY (1, 1) NOT NULL,
    [LabName]               NVARCHAR (50)  NULL,
    [LabPOC]                NVARCHAR (50)  NULL,
    [LabPOCEmail]           NVARCHAR (50)  NULL,
    [SubmissionType]        CHAR (1)       NULL,
    [ReceivedDate]          DATETIME2 (7)  NULL,
    [ProcessedDate]         DATETIME2 (7)  NULL,
    [Status]                TINYINT        NULL,
    [ReviewedDate]          DATETIME2 (7)  NULL,
    [SenderEmailAddress]    NVARCHAR (50)  NULL,
    [ZipFileName]           NVARCHAR (255) NULL,
    [ExtractedFileLocation] NVARCHAR (255) NULL,
    [SubmissionID]          NVARCHAR (100) NULL,
    [ErrorJson]             NVARCHAR (MAX) NULL,
    [Archived]              BIT            CONSTRAINT [DF_SubmissionLog_Archived] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_SubmissionLog] PRIMARY KEY CLUSTERED ([SubmissionLogID] ASC)
);

