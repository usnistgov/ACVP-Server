CREATE TABLE [common].[TaskQueue] (
       [TaskID]            BIGINT         IDENTITY (1, 1)            NOT NULL,
       [TaskType]     NVARCHAR (128)                            NOT NULL,
       [VsId]         BIGINT                                       NOT NULL,       
       [IsSample]     BIT            DEFAULT ((0))              NOT NULL,
       [ShowExpected] BIT            DEFAULT ((0))              NOT NULL,
       [Status]        INT            DEFAULT ((0))              NOT NULL,
       [CreatedOn]    DATETIME2 (7)   NOT NULL DEFAULT CURRENT_TIMESTAMP
);

