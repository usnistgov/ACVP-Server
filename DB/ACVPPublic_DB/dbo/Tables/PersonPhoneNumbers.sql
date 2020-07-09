CREATE TABLE [dbo].[PersonPhoneNumbers] (
    [PersonPhoneNumberId] BIGINT        IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [PersonId]            BIGINT        NOT NULL,
    [OrderIndex]          INT           NOT NULL,
    [PhoneNumber]         NVARCHAR (64) NOT NULL,
    [PhoneNumberType]     NVARCHAR (32) NOT NULL,
    CONSTRAINT [PK_PersonPhoneNumbers] PRIMARY KEY CLUSTERED ([PersonPhoneNumberId] ASC),
    CONSTRAINT [UQ_PersonPhoneNumbers] UNIQUE NONCLUSTERED ([PersonId] ASC, [OrderIndex] ASC)
);

