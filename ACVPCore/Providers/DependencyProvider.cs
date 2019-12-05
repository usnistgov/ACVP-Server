using CVP.DatabaseInterface;
using Mighty;

namespace ACVPCore.Providers
{
	public class DependencyProvider : IDependencyProvider
	{
		private string _acvpConnectionString;

		public DependencyProvider(IConnectionStringFactory connectionStringFactory)
		{
			_acvpConnectionString = connectionStringFactory.GetMightyConnectionString("ACVP");
		}

		public void Delete(long dependencyID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			db.Execute("val.DependencyDelete @0", dependencyID);
		}

		public void DeleteAllAttributes(long dependencyID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			db.Execute("val.DependencyAttributeDeleteAll @0", dependencyID);
		}

		public void DeleteAttribute(long attributeID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			db.Execute("val.DependencyAttributeDelete @0", attributeID);
		}

		public void DeleteAllOELinks(long dependencyID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			db.Execute("val.DependencyOELinkForDependencyDeleteAll @0", dependencyID);
		}

		public void DeleteOELink(long dependencyID, long oeID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			db.Execute("val.DependencyOELinkDelete @0, @1", dependencyID, oeID);
		}
	}
}
