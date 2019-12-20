using System;
using System.Collections.Generic;
using ACVPCore.Results;
using CVP.DatabaseInterface;
using Microsoft.Extensions.Logging;
using Mighty;

namespace ACVPCore.Providers
{
	public class ValidationProvider : IValidationProvider
	{
		private readonly string _acvpConnectionString;
		private readonly ILogger<TestSessionProvider> _logger;

		public ValidationProvider(IConnectionStringFactory connectionStringFactory, ILogger<TestSessionProvider> logger)
		{
			_acvpConnectionString = connectionStringFactory.GetMightyConnectionString("ACVP");
			_logger = logger;
		}

		public InsertResult Insert(long implementationID, bool isLCAVP = false)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var data = db.SingleFromProcedure("val.ValidationRecordInsert", inParams: new
				{
					ImplmenentationId = implementationID,
					SourceId = isLCAVP ? 18 : 1             //TODO - Once LCAVP goes away remove this hardcode to handle this being ACVP (1) vs LCAVP (18)
				});

				return new InsertResult(data.ValidationRecordId);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new InsertResult(ex.Message);
			}
		}

		public List<(long ValidationID, int ValidationSource)> GetValidationsForImplementation(long implementationID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			List<(long ValidationID, int ValidationSource)> validations = new List<(long ValidationID, int ValidationSource)>();
			try
			{
				var data = db.QueryFromProcedure("val.ValidationsForImplementationGet", inParams: new
				{
					ImplementationId = implementationID
				});

				foreach (var validation in data)
				{
					validations.Add((validation.ValidationRecordId, validation.SourceId));
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return validations;
		}
	}
}
