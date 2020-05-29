using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Mighty;

namespace NIST.CVP.Libraries.Shared.ExtensionMethods
{
	public static class MightyOrmExtensionMethods
	{
		public static MightyResultsWithExpando<T> QueryWithExpando<T>(this MightyOrm<T> db,
			string sql, object inParams = null, object outParams = null, bool isProcedure = true)
			where T : class, new()
		{
			var items = new List<T>();
			dynamic resultsExpando;

			using (var cmd = db.CreateCommandWithParams(sql, inParams: inParams, outParams: outParams, isProcedure: isProcedure))
			{
				items = db.Query(cmd).ToList();
				resultsExpando = db.ResultsAsExpando(cmd);
			}

			return new MightyResultsWithExpando<T>(items, resultsExpando);
		}

		public static void ExecuteProcedure(this MightyOrm db, string spName,
			object inParams = null, object outParams = null, object ioParams = null, object returnParams = null,
			DbConnection connection = null,
			int commandTimeout = 30,
			params object[] args)
		{
			using (var cmd = db.CreateCommandWithParams(spName,
				inParams, outParams, ioParams, returnParams,
				isProcedure: true,
				args: args))
			{
				cmd.CommandTimeout = commandTimeout;
				db.Execute(cmd, connection);
			}
		}
	}
}