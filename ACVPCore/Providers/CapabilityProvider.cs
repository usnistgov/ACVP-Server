using System;
using System.Collections.Generic;
using ACVPCore.Models.Capabilities;
using ACVPCore.Results;
using CVP.DatabaseInterface;
using Microsoft.Extensions.Logging;
using Mighty;

namespace ACVPCore.Providers
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

		public InsertResult Insert(long scenarioAlgorithmID, long propertyID, long? rootCapabilityID, long? parentCapabilityID, int level, DatabaseCapabilityType type, int? orderIndex, bool historical, string stringValue, long? numberValue, bool? booleanValue)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var data = db.SingleFromProcedure("val.CapabilityInsert", inParams: new
				{
					ScenarioAlgorithmId = scenarioAlgorithmID,
					PropertyId = propertyID,
					RootCapabilityId = rootCapabilityID,
					ParentCapabilityId = parentCapabilityID,
					Level = level,
					Type = ToCapabilityRecordTypeThatIsNowUseless(type),
					OrderIndex = orderIndex,
					Historical = historical,
					StringValue = stringValue,
					NumberValue = numberValue,
					BooleanValue = booleanValue
				});

				return new InsertResult(data.CapabilityId);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new InsertResult(ex.Message);
			}
		}

		public List<RawCapability> GetRawCapabilitiesForScenarioAlgorithm(long scenarioAlgorithmID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			List<RawCapability> rawCapabilities = new List<RawCapability>();
			try
			{
				var data = db.QueryFromProcedure("val.CapabilitiesForScenarioAlgorithmGet", inParams: new
				{
					ScenarioAlgorithmId = scenarioAlgorithmID
				});

				foreach (var rawCapability in data)
				{
					rawCapabilities.Add(new RawCapability
					{
						ID = rawCapability.CapabilityId,
						ParentCapabilityID = rawCapability.ParentCapabilityId,
						RootCapabilityID = rawCapability.RootCapabilityId,
						PropertyID = rawCapability.PropertyId,
						CapabilityType = ToCapabilityType(rawCapability.CapabilityType),
						Level = rawCapability.Level,
						OrderIndex = rawCapability.OrderIndex,
						StringValue = rawCapability.StringValue,
						NumberValue = rawCapability.NumberValue,
						BooleanValue = rawCapability.BooleanValue
					});
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return rawCapabilities;
		}

		public static DatabaseCapabilityType ToCapabilityType(string value) => value switch
		{
			"AB" => DatabaseCapabilityType.BooleanArray,
			"AC" => DatabaseCapabilityType.CompositeArray,
			"AL" => DatabaseCapabilityType.LongArray,
			"AN" => DatabaseCapabilityType.NumberArray,
			"AO:R" => DatabaseCapabilityType.RangeArray,
			"AS" => DatabaseCapabilityType.StringArray,
			"B" => DatabaseCapabilityType.Boolean,
			"C" => DatabaseCapabilityType.Composite,
			"L" => DatabaseCapabilityType.Long,
			"N" => DatabaseCapabilityType.Number,
			"O:D" => DatabaseCapabilityType.Domain,
			"S" => DatabaseCapabilityType.String
		};

		public static int ToCapabilityRecordTypeThatIsNowUseless(DatabaseCapabilityType type) => type switch
		{
			//This probably isn't even needed anymore, not quite sure how it was used before, not going to use it for anything now...
			//The original types were PRIMITIVE, ARRAY, COMPOSITE, OBJECT
			DatabaseCapabilityType.BooleanArray => 1,
			DatabaseCapabilityType.CompositeArray => 1,
			DatabaseCapabilityType.LongArray => 1,
			DatabaseCapabilityType.NumberArray => 1,
			DatabaseCapabilityType.RangeArray => 3,		//The O in AO:R beats the A, apparently
			DatabaseCapabilityType.StringArray => 1,
			DatabaseCapabilityType.Boolean => 0,
			DatabaseCapabilityType.Composite => 2,
			DatabaseCapabilityType.Long => 0,
			DatabaseCapabilityType.Number => 0,
			DatabaseCapabilityType.Domain => 3,
			DatabaseCapabilityType.String => 0,
			_ => -1 //Garbage, just to get rid of the warning
		};

	}
}
