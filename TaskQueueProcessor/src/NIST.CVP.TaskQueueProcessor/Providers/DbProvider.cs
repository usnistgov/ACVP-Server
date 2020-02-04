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
                    generationTask.Capabilities = GetJson(task.VsId, JsonFileTypes.CAPABILITIES);
                    break;
                case ValidationTask validationTask:
                    validationTask.SubmittedResults = GetJson(task.VsId, JsonFileTypes.SUBMITTED_RESULTS);
                    validationTask.InternalProjection = GetJson(task.VsId, JsonFileTypes.INTERNAL_PROJECTION);
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

        // Only used when building the next task
        private string GetJson(int vsId, string jsonFileType)
        {
            var parameters = new List<(string, object)>
            {
                ("VsId", vsId),
                ("JsonFileType", jsonFileType)
            };

            var reader = RunCommandWithReader(ACVP_DB_NAME, StoredProcedures.GET_JSON, parameters);

            if (!reader.Read())
            {
                throw new Exception($"No JSON found for vsId: {vsId}, jsonType: {jsonFileType}");
            }            
            
            // 3rd element is the Content, could update the SQL Stored Procedure to only provide the actual JSON content because nothing else actually matters here
            var returnContent = reader[2].ToString();
            reader.Close();
            return returnContent;
        }

        public void PutJson(int vsId, string jsonFileType, string jsonContent)
        {
            var parameters = new List<(string, object)>
            {
                ("VsId", vsId),
                ("JsonFileType", jsonFileType),
                ("Content", jsonContent)
            };

            RunCommand(ACVP_DB_NAME, StoredProcedures.PUT_JSON, parameters);
        }
    }
}