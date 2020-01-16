CREATE TABLE [common].[TaskQueue] (
       [TaskID]            BIGINT         IDENTITY (1, 1)            NOT NULL,
       [TaskType]     NVARCHAR (128)                            NOT NULL,
       [VsID]         BIGINT                                       NOT NULL,       
       [IsSample]     BIT            DEFAULT ((0))              NOT NULL,
       [ShowExpected] BIT            DEFAULT ((0))              NOT NULL,
       [Status]        INT            DEFAULT ((0))              NOT NULL,
       [CreatedOn]    DATETIME2 (7)  DEFAULT (sysutcdatetime()) NOT NULL
);

