using System;
using System.Data;
using System.Text;
using NIST.CVP.Common.Interfaces;
using NIST.CVP.Pools.ExtensionMethods;
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

        public DbProvider(IDbConnectionStringFactory connectionStringFactory, IDbConnectionFactory connectionFactory, ITaskRetriever taskRetriever)
        {
            _taskRetriever = taskRetriever;
            _connectionFactory = connectionFactory;
            _connectionStringFactory = connectionStringFactory;
        }

        public ITask GetNextTask()
        {
            ITask task;
            using (var connection = _connectionFactory.Get(_connectionStringFactory.GetConnectionString("Acvp")))
            {
                connection.Open();

                using var command = connection.CreateCommand();
                command.CommandText = StoredProcedures.GET_TASK_QUEUE;
                command.CommandType = CommandType.StoredProcedure;

                var reader = command.ExecuteReader();
                    
                if (reader.Read())
                {
                    task = _taskRetriever.GetTaskFromRow(reader);
                }
                else
                {
                    // If there is no row returned, then there is no data to process, do pool stuff
                    reader.Close();
                    connection.Close();
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
            }

            return task;
        }

        public void DeleteCompletedTask(long taskId)
        {
            using var connection = _connectionFactory.Get(_connectionStringFactory.GetConnectionString("Acvp"));
            connection.Open();
            
            using var command = connection.CreateCommand();
            command.CommandText = StoredProcedures.DELETE_TASK_FROM_TASK_QUEUE;
            command.CommandType = CommandType.StoredProcedure;
            command.AddParameter("id", taskId);            

            command.ExecuteNonQuery();
            connection.Close();
        }

        public void MarkTasksForRestart()
        {
            using var connection = _connectionFactory.Get(_connectionStringFactory.GetConnectionString("Acvp"));
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = StoredProcedures.UPDATE_IN_PROGRESS_TASK_TO_READY;
            command.CommandType = CommandType.StoredProcedure;

            command.ExecuteNonQuery();
            connection.Close();
        }

        private void GetCapabilities(GenerationTask task)
        {
            using var connection = _connectionFactory.Get(_connectionStringFactory.GetConnectionString("Acvp"));
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = StoredProcedures.GET_CAPABILITIES;
            command.CommandType = CommandType.StoredProcedure;
            command.AddParameter("vsId", task.VsId);

            var reader = command.ExecuteReader();

            if (reader.Read())
            {
                task.Capabilities = Encoding.UTF8.GetString((byte[]) reader[0]);
            }
            else
            {
                throw new Exception($"Capabilities could not be found for vsId: {task.VsId}");
            }

            reader.Close();
            connection.Close();
        }

        private void GetResponseData(ValidationTask task)
        {
            using var connection = _connectionFactory.Get(_connectionStringFactory.GetConnectionString("Acvp"));
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = StoredProcedures.GET_SUBMITTED;
            command.CommandType = CommandType.StoredProcedure;
            command.AddParameter("vsId", task.VsId);
            
            var reader = command.ExecuteReader();

            if (reader.Read())
            {
                task.SubmittedResults = Encoding.UTF8.GetString((byte[]) reader[0]);
                task.InternalProjection = Encoding.UTF8.GetString((byte[]) reader[1]);
            }
            else
            {
                throw new Exception($"Response data could not be found for vsId: {task.VsId}");
            }

            reader.Close();
            connection.Close();
        }

        public void PutPromptData(GenerationTask task)
        {
            
        }

        public void PutValidationData(ValidationTask task)
        {
            
        }
    }
}