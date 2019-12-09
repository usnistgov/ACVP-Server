using CVP.DatabaseInterface;
using Mighty;

namespace ACVPCore.Providers
{
	public class VectorSetExpectedResultsProvider : IVectorSetExpectedResultsProvider
	{
		private string _acvpConnectionString;

		public VectorSetExpectedResultsProvider(IConnectionStringFactory connectionStringFactory)
		{
			_acvpConnectionString = connectionStringFactory.GetMightyConnectionString("ACVP");
		}

		public void InsertWithCapabilities(long vectorSetID, string capabilities)
		{
			var db = new MightyOrm(_acvpConnectionString);

			db.Execute("acvp.VectorSetExpectedResultsInsertWithCapabilities @0, @1", vectorSetID, System.Text.Encoding.UTF8.GetBytes(capabilities));
		}
	}
}
