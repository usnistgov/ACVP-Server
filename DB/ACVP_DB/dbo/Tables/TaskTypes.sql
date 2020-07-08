CREATE TABLE [dbo].[TaskTypes]
(
	[TaskTypeId] INT NOT NULL, 
    [Description] NVARCHAR(128) NULL,
	CONSTRAINT [PK_TaskTypes] PRIMARY KEY CLUSTERED ([TaskTypeId] ASC)
)
