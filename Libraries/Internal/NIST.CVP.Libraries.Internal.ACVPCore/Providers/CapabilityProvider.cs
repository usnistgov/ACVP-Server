using System;
using Microsoft.Extensions.Logging;
using Mighty;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
using NIST.CVP.Libraries.Shared.Results;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Providers
{
	public class CapabilityProvider : ICapabilityProvider
	{
		private readonly string _acvpConnectionString;
		private readonly ILogger<TestSessionProvider> _logger;

		public CapabilityProvider(IConnectionStringFactory connectionStringFactory, ILogger<TestSessionProvider> logger)
		{
			_acvpConnectionString = connectionStringFactory.GetMightyConnectionString("ACVP");
			_logger = logger;
		}

		public Result DeleteAllForValidationOEAlgorithm(long validationOEAlgorithmID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.ExecuteProcedure("dbo.CapabilitiesForValidationOEAlgorithmDelete", inParams: new { ValidationOEAlgorithmId = validationOEAlgorithmID });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new Result(ex.Message);
			}

			return new Result();
		}

		public InsertResult Insert(long validationOEAlgorithmID, long propertyID, long? parentCapabilityID, int? orderIndex, bool historical, string stringValue, long? numberValue, bool? booleanValue)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var data = db.SingleFromProcedure("dbo.CapabilityInsert", inParams: new
				{
					ValidationOEAlgorithmId = validationOEAlgorithmID,
					AlgorithmPropertyId = propertyID,
					Historical = historical,
					ParentCapabilityId = parentCapabilityID,
					OrderIndex = orderIndex,
					StringValue = stringValue,
					NumberValue = numberValue,
					BooleanValue = booleanValue
				});

				return new InsertResult(data.CapabilityId);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				_logger.LogError($"{validationOEAlgorithmID} Property: {propertyID} Parent: {parentCapabilityID}" );
				return new InsertResult(ex.Message);
			}
		}
	}
}
