using System;
using System.Collections.Generic;
using ACVPCore.Algorithms.Persisted;
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

		public Result DeleteAllForScenarioAlgorithm(long scenarioAlgorithmID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.Execute("val.CapabilitiesForScenarioAlgorithmDelete @0", scenarioAlgorithmID);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new Result(ex.Message);
			}

			return new Result();
		}

		public InsertResult Insert(long scenarioAlgorithmID, long propertyID, long? rootCapabilityID, long? parentCapabilityID, int level, AlgorithmPropertyType type, int? orderIndex, bool historical, string stringValue, long? numberValue, bool? booleanValue)
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

		public static AlgorithmPropertyType ToCapabilityType(string value) => value switch
		{
			"AB" => AlgorithmPropertyType.BooleanArray,
			"AC" => AlgorithmPropertyType.CompositeArray,
			"AL" => AlgorithmPropertyType.LongArray,
			"AN" => AlgorithmPropertyType.NumberArray,
			"AO:R" => AlgorithmPropertyType.RangeArray,
			"AS" => AlgorithmPropertyType.StringArray,
			"B" => AlgorithmPropertyType.Boolean,
			"C" => AlgorithmPropertyType.Composite,
			"L" => AlgorithmPropertyType.Long,
			"N" => AlgorithmPropertyType.Number,
			"O:D" => AlgorithmPropertyType.Domain,
			"S" => AlgorithmPropertyType.String,
			_ => AlgorithmPropertyType.Boolean		//Garbage and wrong, just to get rid of the warning
		};

		public static int ToCapabilityRecordTypeThatIsNowUseless(AlgorithmPropertyType type) => type switch
		{
			//This probably isn't even needed anymore, not quite sure how it was used before, not going to use it for anything now...
			//The original types were PRIMITIVE, ARRAY, COMPOSITE, OBJECT
			AlgorithmPropertyType.BooleanArray => 1,
			AlgorithmPropertyType.CompositeArray => 1,
			AlgorithmPropertyType.LongArray => 1,
			AlgorithmPropertyType.NumberArray => 1,
			AlgorithmPropertyType.RangeArray => 3,     //The O in AO:R beats the A, apparently
			AlgorithmPropertyType.StringArray => 1,
			AlgorithmPropertyType.Boolean => 0,
			AlgorithmPropertyType.Composite => 2,
			AlgorithmPropertyType.Long => 0,
			AlgorithmPropertyType.Number => 0,
			AlgorithmPropertyType.Domain => 3,
			AlgorithmPropertyType.String => 0,
			_ => -1 //Garbage, just to get rid of the warning
		};

	}
}
