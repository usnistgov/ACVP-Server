CREATE TABLE [common].[TaskQueue] (
       [TaskID]            BIGINT         IDENTITY (1, 1)            NOT NULL,
       [TaskType]     NVARCHAR (128)                            NOT NULL,
       [VsID]         INT                                       NOT NULL,       
       [IsSample]     INT            DEFAULT ((0))              NOT NULL,
       [ShowExpected] INT            DEFAULT ((0))              NOT NULL,
       [Status]        INT            DEFAULT ((0))              NOT NULL,
       [CreatedOn]    DATETIME2 (7)  DEFAULT (sysutcdatetime()) NOT NULL
);

