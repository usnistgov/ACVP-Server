using CVP.DatabaseInterface;
using Mighty;

namespace ACVPCore.Providers
{
	public class VectorSetProvider : IVectorSetProvider
	{
		private string _acvpConnectionString;

		public VectorSetProvider(IConnectionStringFactory connectionStringFactory)
		{
			_acvpConnectionString = connectionStringFactory.GetMightyConnectionString("ACVP");
		}

		public void Cancel(long id)
		{
			var db = new MightyOrm(_acvpConnectionString);

			db.Execute("acvp.VectorSetCancel @0", id);
		}

		public void Insert(long vectorSetID, long testSessionID, string generatorVersion, long algorithmID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			db.Execute("acvp.VectorSetInsert @0, @1, @2, @3", vectorSetID, testSessionID, generatorVersion, algorithmID);
		}

		public void UpdateSubmittedResults(long vectorSetID, string results)
		{
			var db = new MightyOrm(_acvpConnectionString);

			db.Execute("acvp.VectorSetUpdateSubmittedResults @0, @1", vectorSetID, System.Text.Encoding.UTF8.GetBytes(results));
		}
	}
}
