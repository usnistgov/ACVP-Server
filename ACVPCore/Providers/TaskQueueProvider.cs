using CVP.DatabaseInterface;
using Mighty;

namespace ACVPCore.Providers
{
	public class TaskQueueProvider : ITaskQueueProvider
	{
		private string _acvpConnectionString;

		public TaskQueueProvider(IConnectionStringFactory connectionStringFactory)
		{
			_acvpConnectionString = connectionStringFactory.GetMightyConnectionString("ACVP");
		}

		public void Insert(TaskType type, string payload)
		{
			string taskType = type switch
			{
				TaskType.Generation => "vector-generation",
				TaskType.Validation => "vector-validation",
				_ => null
			};

			//TODO throw error if taskType null

			var db = new MightyOrm(_acvpConnectionString);

			db.Execute("common.TaskQueueInsert @0, @1", taskType, System.Text.Encoding.UTF8.GetBytes(payload));
		}
	}
}
