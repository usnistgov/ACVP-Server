﻿CREATE TABLE [acvp].[VECTOR_SET_DATA] (
    [vector_set_id]   BIGINT          NOT NULL,
    [created_on]      DATETIME2 (7)   NOT NULL,
    [data_type]       INT             NOT NULL,
    [vector_set_data] VARBINARY (MAX) NOT NULL,
    [iv_value]        NVARCHAR (10)   NULL,
    CONSTRAINT [PK_VECTOR_SET_DATA] PRIMARY KEY CLUSTERED ([vector_set_id] ASC, [data_type] ASC)
);
