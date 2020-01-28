using System;
using ACVPCore.Results;
using CVP.DatabaseInterface;
using Microsoft.Extensions.Logging;
using Mighty;

namespace ACVPCore.Providers
{
	public class VectorSetExpectedResultsProvider : IVectorSetExpectedResultsProvider
	{
		private readonly string _acvpConnectionString;
		private readonly ILogger<VectorSetExpectedResultsProvider> _logger;

		public VectorSetExpectedResultsProvider(IConnectionStringFactory connectionStringFactory, ILogger<VectorSetExpectedResultsProvider> logger)
		{
			_acvpConnectionString = connectionStringFactory.GetMightyConnectionString("ACVP");
			_logger = logger;
		}

		public Result InsertWithCapabilities(long vectorSetID, string capabilities)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.Execute("acvp.VectorSetExpectedResultsInsertWithCapabilities @0, @1", vectorSetID, System.Text.Encoding.UTF8.GetBytes(capabilities));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new Result(ex.Message);
			}

			return new Result();
		}
	}
}
