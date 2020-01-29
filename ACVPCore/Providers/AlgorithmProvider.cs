using System;
using System.Collections.Generic;
using ACVPCore.Models;
using CVP.DatabaseInterface;
using Microsoft.Extensions.Logging;
using Mighty;

namespace ACVPCore.Providers
{
	public class AlgorithmProvider : IAlgorithmProvider
	{
		private readonly string _acvpConnectionString;
		private readonly ILogger<AlgorithmProvider> _logger;

		public AlgorithmProvider(IConnectionStringFactory connectionStringFactory, ILogger<AlgorithmProvider> logger)
		{
			_acvpConnectionString = connectionStringFactory.GetMightyConnectionString("ACVP");
			_logger = logger;
		}

		public long GetAlgorithmID(string name, string mode)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var data = db.SingleFromProcedure("ref.AlgorithmIDByNameAndModeGet", inParams: new
				{
					Name = name,
					Mode = mode
				});

				return data.AlgorithmId;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return -1;
			}
		}

		public List<AlgorithmLookup> GetAlgorithms()
		{
			var db = new MightyOrm(_acvpConnectionString);

			List<AlgorithmLookup> algorithms = new List<AlgorithmLookup>();

			try
			{
				var data = db.QueryFromProcedure("ref.AlgorithmsGet");

				foreach (var algorithm in data)
				{
					algorithms.Add(new AlgorithmLookup
					{
						AlgorithmID = algorithm.AlgorithmId,
						Name = algorithm.Name,
						Mode = algorithm.Mode,
						Revision = algorithm.Revision,
						DisplayName = algorithm.DisplayName
					});
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
			}

			return algorithms;
		}
	}
}
