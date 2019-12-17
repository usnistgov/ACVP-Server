﻿CREATE TABLE [val].[VALIDATION_CONTACT] (
    [id]                     BIGINT IDENTITY (1, 1) NOT NULL,
    [product_information_id] BIGINT NOT NULL,
    [person_id]              BIGINT NOT NULL,
    [order_index]            INT    NOT NULL,
    CONSTRAINT [PK_VALIDATION_CONTACT] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_VALIDATION_CONTACT_PERSON_ID] FOREIGN KEY ([person_id]) REFERENCES [val].[PERSON] ([id]),
    CONSTRAINT [FK_VALIDATION_CONTACT_PRODUCT_INFORMATION_ID] FOREIGN KEY ([product_information_id]) REFERENCES [val].[PRODUCT_INFORMATION] ([id])
);

