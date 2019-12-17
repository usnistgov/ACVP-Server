CREATE PROCEDURE [acvp].[VectorSetsToArchiveGet]

AS

SET NOCOUNT ON

SELECT V.id, V.[status]
FROM acvp.TEST_SESSION T
	INNER JOIN
	acvp.VECTOR_SET V ON V.test_session_id = T.id
					 AND T.published = 1
					 AND (EXISTS (SELECT NULL FROM acvp.VECTOR_SET_DATA D WHERE D.vector_set_id = V.id)
							OR EXISTS (SELECT NULL FROM acvp.VECTOR_SET_EXPECTED_RESULTS E WHERE E.vector_set_id = V.id))
					 --AND V.archived = 0
					 -- TODO - replace the 2 ugly AND lines with this archived one once I can add the archived field to the vector set table

