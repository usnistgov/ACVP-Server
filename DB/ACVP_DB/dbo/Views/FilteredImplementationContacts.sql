
CREATE VIEW [dbo].[FilteredImplementationContacts]
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
CREATE UNIQUE CLUSTERED INDEX [PK_FilteredImplementationContacts]
    ON [dbo].[FilteredImplementationContacts]([ImplementationContactId] ASC);

