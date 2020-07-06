CREATE TABLE [dbo].[TaskQueue] (
    [TaskId]       BIGINT        IDENTITY (1, 1) NOT NULL,
    [TaskTypeId]   TINYINT       NOT NULL,
    [VectorSetId]  BIGINT        NOT NULL,
    [IsSample]     BIT           CONSTRAINT [DF_TaskQueue_IsSample] DEFAULT ((0)) NOT NULL,
    [ShowExpected] BIT           CONSTRAINT [DF_TaskQueue_ShowExpected] DEFAULT ((0)) NOT NULL,
    [Status]       INT           CONSTRAINT [DF_TaskQueue_Status] DEFAULT ((0)) NOT NULL,
    [CreatedOn]    DATETIME2 (7) CONSTRAINT [DF_TaskQueue_CreatedOn] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_TaskQueue] PRIMARY KEY CLUSTERED ([TaskId] ASC)
);



