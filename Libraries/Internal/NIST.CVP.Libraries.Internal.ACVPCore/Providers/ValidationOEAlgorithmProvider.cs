using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Mighty;
using NIST.CVP.Libraries.Internal.ACVPCore.Models;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
using NIST.CVP.Libraries.Shared.Results;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Providers
{
	public class ValidationOEAlgorithmProvider : IValidationOEAlgorithmProvider
	{
		private readonly string _acvpConnectionString;
		private readonly ILogger<TestSessionProvider> _logger;

		public ValidationOEAlgorithmProvider(IConnectionStringFactory connectionStringFactory, ILogger<TestSessionProvider> logger)
		{
			_acvpConnectionString = connectionStringFactory.GetMightyConnectionString("ACVP");
			_logger = logger;
		}

		public InsertResult Insert(long validationID, long oeID, long algorithmID, long vectorSetID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var data = db.SingleFromProcedure("dbo.ValidationOEAlgorithmInsert", inParams: new
				{
					ValidationId = validationID,
					OEId = oeID,
					AlgorithmId = algorithmID,
					VectorSetId = vectorSetID
				});

				return new InsertResult(data.ValidationOEAlgorithmId);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new InsertResult(ex.Message);
			}
		}

		public List<ValidationOEAlgorithmDisplay> GetActiveValidationOEAlgorithmsForDisplay(long validationID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			List<ValidationOEAlgorithmDisplay> ValidationOEAlgorithms = new List<ValidationOEAlgorithmDisplay>();
			try
			{
				var data = db.QueryFromProcedure("dbo.ValidationOEAlgorithmsForValidationGetActive", inParams: new
				{
					ValidationId = validationID
				});

				foreach (var row in data)
				{
					ValidationOEAlgorithms.Add(new ValidationOEAlgorithmDisplay
					{
						ValidationOEAlgorithmID = row.ValidationOEAlgorithmId,
						AlgorithmDisplayName = row.AlgorithmDisplayName,
						OEID = row.OEId,
						OEDisplay = row.OEName,
						CreatedOn = row.CreatedOn
					});
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return ValidationOEAlgorithms;
		}

		public List<(long ValidationOEAlgorithmID, long AlgorithmID)> GetActiveValidationOEAlgorithms(long validationID, long oeID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			List<(long ValidationOEAlgorithmID, long AlgorithmID)> ValidationOEAlgorithms = new List<(long ValidationOEAlgorithmID, long AlgorithmID)>();
			try
			{
				var data = db.QueryFromProcedure("dbo.ValidationOEAlgorithmsForOEGetActive", inParams: new
				{
					ValidationId = validationID,
					OEId = oeID
				});

				foreach (var row in data)
				{
					ValidationOEAlgorithms.Add((row.ValidationOEAlgorithmId, row.AlgorithmId));
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return ValidationOEAlgorithms;
		}

		public Result Inactivate(long validationOEAlgorithmID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.ExecuteProcedure("dbo.ValidationOEAlgorithmInactivate", inParams: new { ValidationOEAlgorithmId = validationOEAlgorithmID });
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
