using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Mighty;
using NIST.CVP.Libraries.Internal.ACVPCore.Models;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
using NIST.CVP.Libraries.Shared.ExtensionMethods;
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
				_logger.LogError(ex);
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
				_logger.LogError(ex);
				_logger.LogError($"{validationOEAlgorithmID} Property: {propertyID} Parent: {parentCapabilityID}" );
				return new InsertResult(ex.Message);
			}
		}

		public List<RawCapability> GetCapabilities(long validationOEAlgorithmID)
		{
			List<RawCapability> capabilities = new List<RawCapability>();

			var db = new MightyOrm(_acvpConnectionString);

			try {
				var data = db.QueryFromProcedure("dbo.CapabilitiesGet", inParams: new
				{
					ValidationOEAlgorithmId = validationOEAlgorithmID,
				});

				foreach (var row in data)
				{
					capabilities.Add(new RawCapability
					{
						ID = row.CapabilityId,
						AlgorithmPropertyID = row.AlgorithmPropertyId,
						ParentCapabilityID = row.ParentCapabilityId,
						HistoricalCapability = row.HistoricalCapability,
						CapabilityOrderIndex = row.CapabilityOrderIndex,
						BooleanValue = row.BooleanValue,
						StringValue = row.StringDisplayValue,
						NumberValue = row.NumberValue,
						PropertyDisplayName = row.DisplayName,
						PropertyType = (AlgorithmPropertyType)row.AlgorithmPropertyTypeId,
						HistoricalProperty = row.HistoricalProperty,
						IsRequired = row.IsRequired ?? false,
						UnitsLabel = row.UnitsLabel,
						PropertyOrderIndex = row.PropertyOrderIndex,
						HistoricalAlgorithm = row.HistoricalAlgorithm
					});
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex);
			}

			return capabilities;
		}
	}
}
