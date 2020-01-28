CREATE TABLE [val].[ORGANIZATION] (
    [id]                     BIGINT          IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [parent_organization_id] BIGINT          NULL,
    [name]                   NVARCHAR (1024) NOT NULL,
    [organization_url]       NVARCHAR (1024) NULL,
    [voice_number]           NVARCHAR (64)   NULL,
    [fax_number]             NVARCHAR (64)   NULL,
    CONSTRAINT [PK_ORGANIZATION] PRIMARY KEY CLUSTERED ([id] ASC)
);

