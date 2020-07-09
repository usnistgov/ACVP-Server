CREATE TABLE [dbo].[Implementations] (
    [ImplementationId]          BIGINT          NOT NULL,
    [OrganizationId]            BIGINT          NOT NULL,
    [AddressId]                 BIGINT          NOT NULL,
    [Url]                       NVARCHAR (1024) NULL,
    [ImplementationName]        NVARCHAR (1024) NOT NULL,
    [ImplementationTypeId]      INT             NOT NULL,
    [ImplementationVersion]     NVARCHAR (128)  NOT NULL,
    [ImplementationDescription] NVARCHAR (MAX)  NOT NULL,
    [ITAR]                      BIT             NOT NULL
);


GO
CREATE UNIQUE CLUSTERED INDEX [PK_CSRC_Implementations]
    ON [dbo].[Implementations]([ImplementationId] ASC);

