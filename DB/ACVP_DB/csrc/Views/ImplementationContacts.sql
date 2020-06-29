
CREATE VIEW [csrc].[ImplementationContacts]
WITH SCHEMABINDING
AS

SELECT	 C.ImplementationContactId
		,C.ImplementationId
		,C.PersonId
		,C.OrderIndex
FROM dbo.ImplementationContacts C
	INNER JOIN
	dbo.Implementations I ON I.ImplementationId = C.ImplementationId
							AND I.ITAR = 0


GO
CREATE UNIQUE CLUSTERED INDEX [PK_CSRC_VALIDATION_CONTACT]
    ON [csrc].[ImplementationContacts]([ImplementationContactId] ASC);

