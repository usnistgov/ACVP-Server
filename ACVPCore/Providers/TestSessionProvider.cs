using CVP.DatabaseInterface;
using Mighty;

namespace ACVPCore.Providers
{
	public class TestSessionProvider : ITestSessionProvider
	{
		private string _acvpConnectionString;

		public TestSessionProvider(IConnectionStringFactory connectionStringFactory)
		{
			_acvpConnectionString = connectionStringFactory.GetMightyConnectionString("ACVP");
		}

		public void Cancel(long id)
		{
			var db = new MightyOrm(_acvpConnectionString);

			db.Execute("acvp.TestSessionCancel @0", id);
		}

		public void CancelVectorSets(long id)
		{
			var db = new MightyOrm(_acvpConnectionString);

			db.Execute("acvp.TestSessionVectorSetsCancel @0", id);
		}

		public void Insert(long testSessionId, int acvVersionID, string generator, bool isSample, bool publishable, long userID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			db.Execute("acvp.TestSessionInsert @0, @1, @2, @3, @4, @5", testSessionId, acvVersionID, generator, isSample, publishable, userID);
		}
	}
}
