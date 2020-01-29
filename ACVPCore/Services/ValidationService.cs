using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using ACVPCore.Algorithms;
using ACVPCore.Algorithms.DataTypes;
using ACVPCore.Algorithms.External;
using ACVPCore.Algorithms.Persisted;
using ACVPCore.Models;
using ACVPCore.Providers;
using ACVPCore.Results;

namespace ACVPCore.Services
{
	public class ValidationService : IValidationService
	{
		IValidationProvider _validationProvider;
		IScenarioProvider _scenarioProvider;
		IScenarioOEProvider _scenarioOEProvider;
		IScenarioAlgorithmProvider _scenarioAlgorithmProvider;
		ICapabilityProvider _capabilityProvider;
		ICapabilityService _capabilityService;
		IPropertyService _propertyService;

		public ValidationService(IValidationProvider validationProvider, IScenarioProvider scenarioProvider, IScenarioOEProvider scenarioOEProvider, IScenarioAlgorithmProvider scenarioAlgorithmProvider, ICapabilityProvider capabilityProvider, ICapabilityService capabilityService, IPropertyService propertyService)
		{
			_validationProvider = validationProvider;
			_scenarioProvider = scenarioProvider;
			_scenarioOEProvider = scenarioOEProvider;
			_scenarioAlgorithmProvider = scenarioAlgorithmProvider;
			_capabilityProvider = capabilityProvider;
			_capabilityService = capabilityService;
			_propertyService = propertyService;
		}

		public InsertResult Create(ValidationSource validationSource, long implementationID)
		{
			//Get the validation number to assign to it
			long validationNumber = GetValidationNumber(validationSource);

			if (validationNumber == -1) return new InsertResult("Failed to get new validation number");

			//Create the validation record
			InsertResult validationInsertResult = _validationProvider.Insert(validationSource, validationNumber, implementationID);

			return validationInsertResult;
		}

		public InsertResult CreateScenario(long validationID, long oeID)
		{
			//Create the scenario record
			InsertResult scenarioCreateResult = AddScenarioToValidation(validationID);

			if (!scenarioCreateResult.IsSuccess) return scenarioCreateResult;

			long scenarioID = scenarioCreateResult.ID;

			//Create the scenario OE link
			Result OEResult = AddOEToScenario(scenarioID, oeID);

			return OEResult.IsSuccess ? scenarioCreateResult : (InsertResult)OEResult;
		}

		public long GetScenarioIDForValidationOE(long validationID, long oeID) => _scenarioProvider.GetScenarioIDForValidationOE(validationID, oeID);

		private InsertResult AddScenarioToValidation(long validationID) => _scenarioProvider.Insert(validationID);

		private Result AddOEToScenario(long scenarioID, long oeID) => _scenarioOEProvider.Insert(scenarioID, oeID);

		public InsertResult AddScenarioAlgorithm(long scenarioID, long algorithmID) => _scenarioAlgorithmProvider.Insert(scenarioID, algorithmID);

		public void DeleteScenarioAlgorithm(long scenarioAlgorithmID)
		{
			//Delete the capabilities
			Result capabilitiesDeleteResult = _capabilityProvider.DeleteAllForScenarioAlgorithm(scenarioAlgorithmID);

			//Delete the prereqs
			//TODO - prereqs!

			//Delete the scenario algorithm
			Result scenarioAlgorithmDeleteResult = _scenarioAlgorithmProvider.Delete(scenarioAlgorithmID);
		}

		public List<(long ScenarioAlgorithmID, long AlgorithmID)> GetScenarioAlgorithms(long scenarioID) => _scenarioAlgorithmProvider.GetScenarioAlgorithmsForScenario(scenarioID);

		public List<(long ValidationID, int ValidationSource)> GetValidationsForImplementation(long implementationID) => _validationProvider.GetValidationsForImplementation(implementationID);

		//Get the most recently created ACVP validation ID, or 0 if none exists
		public long GetLatestACVPValidationForImplementation(long implementationID) => GetValidationsForImplementation(implementationID).Where(x => x.ValidationSource == 1).DefaultIfEmpty().Max(x => x.ValidationID);

		public long GetValidationNumber(ValidationSource validationSource) => validationSource switch
		{
			ValidationSource.ACVP => _validationProvider.GetNextACVPValidationNumber(),
			ValidationSource.LCAVP => _validationProvider.GetNextLCAVPValidationNumber(),
			_ => -1
		};

		public Result LogValidationTestSession(long validationID, long testSessionID) => _validationProvider.ValidationTestSessionInsert(validationID, testSessionID);

		public void PersistCapabilities(long algorithmID, long scenarioAlgorithmID, IExternalAlgorithm externalAlgorithm)
		{
			//Convert it to a persistence algorithm
			IPersistedAlgorithm persistenceAlgorithm = PersistedAlgorithmFactory.GetPersistedAlgorithm(externalAlgorithm);

			//Persist it - the entire algorithm object is just a class as far as the persistence mechanism is concerned, just with some non-property properties on it
			PersistClassCapabilities(algorithmID, scenarioAlgorithmID, null, null, 0, 0, persistenceAlgorithm);
		}

		public void PersistClassCapabilities(long algorithmID, long scenarioAlgorithmID, long? rootCapabilityID, long? parentCapabilityID, int level, int orderIndex, Object objectClass)
		{
			//Iterate through each property of the class
			foreach (PropertyInfo prop in (objectClass.GetType()).GetProperties())
			{
				//Try to get our custom attribute off this property - some won't have one (really more on the algorithm class)
				AlgorithmProperty algorithmProperty = (AlgorithmProperty)Attribute.GetCustomAttribute(prop, typeof(AlgorithmProperty));

				//If it has that custom attribute then need to persist the capability
				if (algorithmProperty != null)
				{
					//Look up the property to get the ID
					PropertyLookup propertyLookup = _propertyService.LookupProperty(algorithmID, algorithmProperty.Name);
					long propertyID = propertyLookup.PropertyID;

					//Get the actual property object. This will be a long, string, bool, List<something>, Range, Domain, or another class instance
					Object classProperty = prop.GetValue(objectClass);

					//Persist
					switch (algorithmProperty.Type)
					{
						case AlgorithmPropertyType.Boolean:
							PersistBooleanCapability(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, orderIndex, (bool)classProperty);
							break;

						case AlgorithmPropertyType.BooleanArray: 
							PersistBooleanArrayCapability(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, orderIndex, (List<bool>)classProperty); 
							break;

						case AlgorithmPropertyType.Composite:
							PersistCompositeCapability(algorithmID, scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, orderIndex, classProperty);
							break;

						case AlgorithmPropertyType.CompositeArray:
							PersistCompositeArrayCapability(algorithmID, scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, orderIndex, (List<object>)classProperty);
							break;

						case AlgorithmPropertyType.Domain:
							PersistDomainCapability(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, orderIndex, (Domain)classProperty);
							break;

						case AlgorithmPropertyType.Long:
							PersistLongCapability(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, orderIndex, (long)classProperty);
							break;

						case AlgorithmPropertyType.LongArray:
							PersistLongArrayCapability(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, orderIndex, (List<long>)classProperty);
							break;

						case AlgorithmPropertyType.Number:
							PersistNumberCapability(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, orderIndex, (long)classProperty);
							break;

						case AlgorithmPropertyType.NumberArray:
							PersistNumberArrayCapability(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, orderIndex, (List<long>)classProperty);
							break;

						case AlgorithmPropertyType.Range:
							PersistRangeCapability(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, orderIndex, (Algorithms.DataTypes.Range)classProperty);
							break;

						case AlgorithmPropertyType.RangeArray:
							PersistRangeArrayCapability(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, orderIndex, (List<Algorithms.DataTypes.Range>)classProperty);
							break;

						case AlgorithmPropertyType.String:
							PersistStringCapability(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, orderIndex, (string)classProperty);
							break;

						case AlgorithmPropertyType.StringArray:
							PersistStringArrayCapability(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, orderIndex, (List<string>)classProperty);
							break;
					}
				}
			}
		}

		public void PersistBooleanCapability(long scenarioAlgorithmID, long propertyID, long? rootCapabilityID, long? parentCapabilityID, int level, int orderIndex, bool value)
		{
			_capabilityProvider.Insert(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, AlgorithmPropertyType.Boolean, orderIndex, false, null, null, value);
		}

		public void PersistLongCapability(long scenarioAlgorithmID, long propertyID, long? rootCapabilityID, long? parentCapabilityID, int level, int orderIndex, long value)
		{
			_capabilityProvider.Insert(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, AlgorithmPropertyType.Long, orderIndex, false, null, value, null);
		}

		public void PersistNumberCapability(long scenarioAlgorithmID, long propertyID, long? rootCapabilityID, long? parentCapabilityID, int level, int orderIndex, long value)
		{
			_capabilityProvider.Insert(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, AlgorithmPropertyType.Number, orderIndex, false, null, value, null);
		}

		public void PersistStringCapability(long scenarioAlgorithmID, long propertyID, long? rootCapabilityID, long? parentCapabilityID, int level, int orderIndex, string value)
		{
			_capabilityProvider.Insert(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, AlgorithmPropertyType.String, orderIndex, false, value, null, null);
		}

		public void PersistRangeCapability(long scenarioAlgorithmID, long propertyID, long? rootCapabilityID, long? parentCapabilityID, int level, int orderIndex, ACVPCore.Algorithms.DataTypes.Range value)
		{
			string stringValue = JsonSerializer.Serialize(value, new JsonSerializerOptions { IgnoreNullValues = true });
			_capabilityProvider.Insert(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, AlgorithmPropertyType.Range, orderIndex, false, stringValue, null, null);
		}

		public void PersistDomainCapability(long scenarioAlgorithmID, long propertyID, long? rootCapabilityID, long? parentCapabilityID, int level, int orderIndex, Domain value)
		{
			//Need to turn the segments into a string that is a JSON array of numbers and Range objects. This could probably be done somehow in the DomainConverter, but just doing it here for now 
			List<string> segmentsAsStrings = new List<string>();

			foreach (var segment in value.Segments)
			{
				if (segment is NumericSegment) { segmentsAsStrings.Add(((NumericSegment)segment).Value.ToString()); }
				else { segmentsAsStrings.Add(JsonSerializer.Serialize((Algorithms.DataTypes.Range)segment, new JsonSerializerOptions { IgnoreNullValues = true })); }
			}

			//Join the collection with a comma delimiter and put brackets around it => looks like a JSON array
			string stringValue = $"[{String.Join(",", segmentsAsStrings)}]";

			_capabilityProvider.Insert(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, AlgorithmPropertyType.Domain, orderIndex, false, stringValue, null, null);
		}

		public void PersistCompositeCapability(long algorithmID, long scenarioAlgorithmID, long propertyID, long? rootCapabilityID, long? parentCapabilityID, int level, int orderIndex, object value)
		{
			//Create the container capability
			InsertResult result = _capabilityProvider.Insert(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, AlgorithmPropertyType.Composite, orderIndex, false, null, null, null);

			//Grab the ID of the container that was just created, it will be the parent of the child capabilities
			long containerCapabilityID = result.ID;

			//If this object was a level 0 object, then the root for the children is the container
			if (level == 0) rootCapabilityID = containerCapabilityID;

			//Create the child capabilities from this object
			PersistClassCapabilities(algorithmID, scenarioAlgorithmID, rootCapabilityID, containerCapabilityID, level + 1, orderIndex, value);
		}


		public void PersistBooleanArrayCapability(long scenarioAlgorithmID, long propertyID, long? rootCapabilityID, long? parentCapabilityID, int level, int orderIndex, List<bool> values)
		{
			//Create the container capability
			InsertResult result = _capabilityProvider.Insert(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, AlgorithmPropertyType.BooleanArray, orderIndex, false, null, null, null);

			//Grab the ID of the container that was just created, it will be the parent of the value capabilities
			long containerCapabilityID = result.ID;

			//If this object was a level 0 object, then the root for the children is the container
			if (level == 0) rootCapabilityID = containerCapabilityID;

			//Create the value capabilities
			for (int i = 0; i < values.Count; i++)
			{
				PersistBooleanCapability(scenarioAlgorithmID, propertyID, rootCapabilityID, containerCapabilityID, level + 1, i, values[i]);
			}
		}

		public void PersistLongArrayCapability(long scenarioAlgorithmID, long propertyID, long? rootCapabilityID, long? parentCapabilityID, int level, int orderIndex, List<long> values)
		{
			//Create the container capability
			InsertResult result = _capabilityProvider.Insert(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, AlgorithmPropertyType.LongArray, orderIndex, false, null, null, null);

			//Grab the ID of the container that was just created, it will be the parent of the value capabilities
			long containerCapabilityID = result.ID;

			//If this object was a level 0 object, then the root for the children is the container
			if (level == 0) rootCapabilityID = containerCapabilityID;

			//Create the value capabilities
			for (int i = 0; i < values.Count; i++)
			{
				PersistLongCapability(scenarioAlgorithmID, propertyID, rootCapabilityID, containerCapabilityID, level + 1, i, values[i]);
			}
		}

		public void PersistNumberArrayCapability(long scenarioAlgorithmID, long propertyID, long? rootCapabilityID, long? parentCapabilityID, int level, int orderIndex, List<long> values)
		{
			//Create the container capability
			InsertResult result = _capabilityProvider.Insert(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, AlgorithmPropertyType.NumberArray, orderIndex, false, null, null, null);

			//Grab the ID of the container that was just created, it will be the parent of the value capabilities
			long containerCapabilityID = result.ID;

			//If this object was a level 0 object, then the root for the children is the container
			if (level == 0) rootCapabilityID = containerCapabilityID;

			//Create the value capabilities
			for (int i = 0; i < values.Count; i++)
			{
				PersistNumberCapability(scenarioAlgorithmID, propertyID, rootCapabilityID, containerCapabilityID, level + 1, i, values[i]);
			}
		}

		public void PersistStringArrayCapability(long scenarioAlgorithmID, long propertyID, long? rootCapabilityID, long? parentCapabilityID, int level, int orderIndex, List<string> values)
		{
			//Create the container capability
			InsertResult result = _capabilityProvider.Insert(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, AlgorithmPropertyType.StringArray, orderIndex, false, null, null, null);

			//Grab the ID of the container that was just created, it will be the parent of the value capabilities
			long containerCapabilityID = result.ID;

			//If this object was a level 0 object, then the root for the children is the container
			if (level == 0) rootCapabilityID = containerCapabilityID;

			//Create the value capabilities
			for (int i = 0; i < values.Count; i++)
			{
				PersistStringCapability(scenarioAlgorithmID, propertyID, rootCapabilityID, containerCapabilityID, level + 1, i, values[i]);
			}
		}

		public void PersistRangeArrayCapability(long scenarioAlgorithmID, long propertyID, long? rootCapabilityID, long? parentCapabilityID, int level, int orderIndex, List<ACVPCore.Algorithms.DataTypes.Range> values)
		{
			//Create the container capability
			InsertResult result = _capabilityProvider.Insert(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, AlgorithmPropertyType.RangeArray, orderIndex, false, null, null, null);

			//Grab the ID of the container that was just created, it will be the parent of the value capabilities
			long containerCapabilityID = result.ID;

			//If this object was a level 0 object, then the root for the children is the container
			if (level == 0) rootCapabilityID = containerCapabilityID;

			//Create the child range capabilities
			for (int i = 0; i < values.Count; i++)
			{
				PersistRangeCapability(scenarioAlgorithmID, propertyID, rootCapabilityID, containerCapabilityID, level + 1, i, values[i]);
			}
		}

		public void PersistCompositeArrayCapability(long algorithmID, long scenarioAlgorithmID, long propertyID, long? rootCapabilityID, long? parentCapabilityID, int level, int orderIndex, List<object> values)
		{
			//Create the container capability
			InsertResult result = _capabilityProvider.Insert(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, AlgorithmPropertyType.CompositeArray, orderIndex, false, null, null, null);

			//Grab the ID of the container that was just created, it will be the parent of the value capabilities
			long containerCapabilityID = result.ID;

			//If this object was a level 0 object, then the root for the children is the container
			if (level == 0) rootCapabilityID = containerCapabilityID;

			//Create the child composite capabilities
			for (int i = 0; i < values.Count; i++)
			{
				PersistCompositeCapability(algorithmID, scenarioAlgorithmID, propertyID, rootCapabilityID, containerCapabilityID, level + 1, i, values[i]);
			}
		}

	}
}
