using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using NIST.CVP.TaskQueueProcessor.Constants;
using NIST.CVP.TaskQueueProcessor.TaskModels;

namespace NIST.CVP.TaskQueueProcessor.Providers
{
    public class DbProvider : IDbProvider
    {
        // TODO Should this be made static with locks on the DB ? 
        private readonly SqlConnection _sql;
        private readonly TaskRetriever _taskRetriever = new TaskRetriever();
        private readonly string _poolUrl;
        private readonly int _poolPort;
        private readonly bool _allowPoolSpawn;

        public DbProvider(string connectionString, string poolUrl, int poolPort, bool allowPoolSpawn)
        {
            _poolUrl = poolUrl;
            _poolPort = poolPort;
            _allowPoolSpawn = allowPoolSpawn;
            Console.WriteLine($"Connecting to database with connectionString: {connectionString}");

            try
            {
                _sql = new SqlConnection(connectionString);
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine(sqlEx);
            }
        }

        public ITask GetNextTask()
        {
            ITask task;
            _sql.Open();

            using var command = new SqlCommand
            {
                CommandText = StoredProcedures.GET_TASK_QUEUE,
                CommandType = CommandType.StoredProcedure,
                Connection = _sql
            };

            var reader = command.ExecuteReader();

            if (reader.Read())
            {
                task = _taskRetriever.GetTaskFromRow(reader);
            }
            else
            {
                // If there is no row returned, then there is no data to process, do pool stuff
                task = new PoolTask(new PoolProvider(_poolUrl, _poolPort), _allowPoolSpawn); // TODO these should be removed from being parameters and into DI
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
            
            _sql.Close();
            return task;
        }

        public void DeleteCompletedTask(int taskId)
        {
            _sql.Open();

            using var command = new SqlCommand
            {
                CommandText = StoredProcedures.DELETE_TASK_FROM_TASK_QUEUE,
                CommandType = CommandType.StoredProcedure,
                Connection = _sql,
                Parameters = {"id", taskId}
            };

            var reader = command.ExecuteReader();

            if (reader.RecordsAffected != 1)
            {
                throw new Exception("Incorrect number of rows affected by DELETE TASK FROM TASK QUEUE");
            }
            
            reader.Close();
            _sql.Close();
        }

        public void MarkTasksForRestart()
        {
            _sql.Open();

            using var command = new SqlCommand
            {
                CommandText = StoredProcedures.UPDATE_IN_PROGRESS_TASK_TO_READY,
                CommandType = CommandType.StoredProcedure,
                Connection = _sql
            };

            var reader = command.ExecuteReader();
            reader.Close();
            _sql.Close();
        }

        private void GetCapabilities(GenerationTask task)
        {
            using var getCapabilities = new SqlCommand
            {
                CommandText = StoredProcedures.GET_CAPABILITIES,
                CommandType = CommandType.StoredProcedure,
                Connection = _sql,
                Parameters = { "vsId", task.VsId }
            };
            
            var reader = getCapabilities.ExecuteReader();

            if (reader.Read())
            {
                task.Capabilities = Encoding.UTF8.GetString((byte[]) reader[0]);
            }
            else
            {
                throw new Exception("Capabilities could not be found");
            }

            reader.Close();
        }

        private void GetResponseData(ValidationTask task)
        {
            using var getResponseData = new SqlCommand
            {
                CommandText = StoredProcedures.GET_SUBMITTED,
                CommandType = CommandType.StoredProcedure,
                Connection = _sql,
                Parameters = { "vsId", task.VsId }
            };
            
            var reader = getResponseData.ExecuteReader();

            if (reader.Read())
            {
                task.SubmittedResults = Encoding.UTF8.GetString((byte[]) reader[0]);
                task.InternalProjection = Encoding.UTF8.GetString((byte[]) reader[1]);
            }
            else
            {
                throw new Exception("Response data could not be found");
            }

            reader.Close();
        }
    }
}