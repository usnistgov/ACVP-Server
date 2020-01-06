using System;
using System.Collections.Generic;
using System.Data;
using NIST.CVP.Common.Interfaces;
using NIST.CVP.Pools.ExtensionMethods;    // TODO move up somewhere more generic?
using NIST.CVP.TaskQueueProcessor.Constants;
using NIST.CVP.TaskQueueProcessor.TaskModels;

namespace NIST.CVP.TaskQueueProcessor.Providers
{
    public class DbProvider : IDbProvider
    {
        // TODO Should this be made static with locks on the DB ? 
        private readonly ITaskRetriever _taskRetriever;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IDbConnectionStringFactory _connectionStringFactory;

        private const string ACVP_DB_NAME = "Acvp";
        
        public DbProvider(IDbConnectionStringFactory connectionStringFactory, IDbConnectionFactory connectionFactory, ITaskRetriever taskRetriever)
        {
            _taskRetriever = taskRetriever;
            _connectionFactory = connectionFactory;
            _connectionStringFactory = connectionStringFactory;
        }

        private IDataReader RunCommandWithReader(string db, string procedure, IEnumerable<(string, object)> parameters = null)
        {
            var connection = _connectionFactory.Get(_connectionStringFactory.GetConnectionString(db));
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = procedure;
            command.CommandType = CommandType.StoredProcedure;
            command.AddParameters(parameters);

            return command.ExecuteReader(CommandBehavior.CloseConnection);    // Close connection when the reader is done reading
        }

        private void RunCommand(string db, string procedure, IEnumerable<(string, object)> parameters = null)
        {
            using var connection = _connectionFactory.Get(_connectionStringFactory.GetConnectionString(db));
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = procedure;
            command.CommandType = CommandType.StoredProcedure;
            command.AddParameters(parameters);
            
            var rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected == 0)
            {
                // Error notice?
                // Is this always an error?
            }
            
            connection.Close();
        }

        public ITask GetNextTask()
        {
            ITask task;
            var reader = RunCommandWithReader(ACVP_DB_NAME, StoredProcedures.GET_TASK_QUEUE);
                
            if (reader.Read())
            {
                task = _taskRetriever.GetTaskFromRow(reader, this);
            }
            else
            {
                // If there is no row returned, then there is no data to process, do pool stuff
                reader.Close();
                return null;
            }

            reader.Close();

            switch (task)
            {
                case GenerationTask generationTask:
                    GetCapabilities(generationTask);
                    break;
                case ValidationTask validationTask:
                    GetResponseData(validationTask);
                    break;
            }
            
            return task;
        }

        public void DeleteCompletedTask(long taskId)
        {
            var parameters = new List<(string, object)>
            {
                ("TaskID", taskId)
            };
            
            RunCommand(ACVP_DB_NAME, StoredProcedures.DELETE_TASK_FROM_TASK_QUEUE, parameters);
        }

        public void MarkTasksForRestart()
        {
            RunCommand(ACVP_DB_NAME, StoredProcedures.UPDATE_IN_PROGRESS_TASK_TO_READY);
        }

        private void GetCapabilities(GenerationTask task)
        {
            var parameters = new List<(string, object)>
            {
                ("VsID", task.VsId)
            };

            var reader = RunCommandWithReader(ACVP_DB_NAME, StoredProcedures.GET_CAPABILITIES, parameters);
            
            if (reader.Read())
            {
                task.Capabilities = reader[0].ToString();
            }
            else
            {
                throw new Exception($"Capabilities could not be found for vsId: {task.VsId}");
            }

            reader.Close();
        }

        private void GetResponseData(ValidationTask task)
        {
            var parameters = new List<(string, object)>
            {
                ("VsID", task.VsId)
            };

            var reader = RunCommandWithReader(ACVP_DB_NAME, StoredProcedures.GET_SUBMITTED, parameters);

            if (reader.Read())
            {
                task.SubmittedResults = reader[0].ToString();
                task.InternalProjection = reader[1].ToString();
            }
            else
            {
                throw new Exception($"Response data could not be found for vsId: {task.VsId}");
            }

            reader.Close();
        }

        public void PutPromptData(GenerationTask task)
        {
            var parameters = new List<(string, object)>
            {
                ("VsID", task.VsId),
                ("Prompt", task.Prompt),
                ("InternalProjection", task.InternalProjection),
                ("ExpectedResults", task.ExpectedResults)
            };
            
            RunCommand(ACVP_DB_NAME, StoredProcedures.PUT_ALL_PROMPT_DATA, parameters);
        }

        public void PutValidationData(ValidationTask task)
        {
            var parameters = new List<(string, object)>
            {
                ("VsID", task.VsId),
                ("Prompt", task.Validation),
            };
            
            RunCommand(ACVP_DB_NAME, StoredProcedures.PUT_VALIDATION, parameters);
        }

        public void PutErrorData(ExecutableTask task)
        {
            var parameters = new List<(string, object)>
            {
                ("Error", task.Error)
            };
            
            RunCommand(ACVP_DB_NAME, StoredProcedures.PUT_ERROR, parameters);
        }
    }
}