
CREATE VIEW [csrc].[PersonPhoneNumbers]
WITH SCHEMABINDING
AS

SELECT PersonPhoneNumberId, PersonId, OrderIndex, PhoneNumber, PhoneNumberType
FROM dbo.PersonPhoneNumbers


GO
CREATE UNIQUE CLUSTERED INDEX [PK_CSRC_PersonPhoneNumbers]
    ON [csrc].[PersonPhoneNumbers]([PersonPhoneNumberId] ASC);

