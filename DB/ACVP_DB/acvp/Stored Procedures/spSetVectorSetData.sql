
-- Performs and INSERT or UPDATE as needed to set the vector set data
CREATE PROCEDURE [acvp].[spSetVectorSetData]
	   @vector_set_id BIGINT
	  , @data_type INT
	  , @vector_set_data varbinary(max)
AS
	IF EXISTS (SELECT vector_set_id FROM acvp.VECTOR_SET_DATA WHERE vector_set_id = @vector_set_id AND data_type = @data_type)
		UPDATE acvp.VECTOR_SET_DATA SET vector_set_data = @vector_set_data WHERE vector_set_id = @vector_set_id AND data_type = @data_type
	ELSE
		INSERT INTO acvp.VECTOR_SET_DATA (vector_set_id, vector_set_data, data_type) VALUES (@vector_set_id, @vector_set_data, @data_type)

