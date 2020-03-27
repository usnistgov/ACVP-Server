using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using ACVPCore.Models;
using ACVPCore.Providers;
using NIST.CVP.Algorithms.DataTypes;
using NIST.CVP.Algorithms.Persisted;
using NIST.CVP.Results;

namespace ACVPCore.Services
{
	public class CapabilityService : ICapabilityService
	{
		private readonly ICapabilityProvider _capabilityProvider;
		private readonly IPropertyService _propertyService;

		public CapabilityService(ICapabilityProvider capabilityProvider, IPropertyService propertyService)
		{
			_capabilityProvider = capabilityProvider;
			_propertyService = propertyService;
		}

		public Result DeleteAllForScenarioAlgorithm(long scenarioAlgorithmID) => _capabilityProvider.DeleteAllForScenarioAlgorithm(scenarioAlgorithmID);

		public void CreateClassCapabilities(long algorithmID, long scenarioAlgorithmID, long? rootCapabilityID, long? parentCapabilityID, int level, int orderIndex, string parentPropertyName, Object objectClass)
		{
			//Iterate through each property of the class
			foreach (PropertyInfo prop in (objectClass.GetType()).GetProperties())
			{
				//Try to get our custom attribute off this property - some won't have one (really more on the algorithm class)
				AlgorithmProperty algorithmProperty = (AlgorithmProperty)Attribute.GetCustomAttribute(prop, typeof(AlgorithmProperty));

				//If it has that custom attribute then need to persist the capability
				if (algorithmProperty != null)
				{
					//Get the actual property value. This will be a long, string, bool, List<something>, Range, Domain, or another class instance, at least once cast...
					Object classProperty = prop.GetValue(objectClass);

					//If the property is a nullable type we may have a null, in which case skip this property because we don't need to create a capability for it
					if (classProperty == null)
					{
						continue;
					}

					//If a nested name property, prepend the parent name to what is defined on the attribute. This allows reuse of the same class in multiple places in an algorithm's property tree - very useful in KAS
					string propertyName = algorithmProperty.PrependParentPropertyName ? $"{parentPropertyName}/{algorithmProperty.Name}" : algorithmProperty.Name;

					//Get the property ID
					PropertyLookup propertyLookup = _propertyService.LookupProperty(algorithmID, propertyName);
					long propertyID = propertyLookup.PropertyID;

					//Create the capability based on the type of the property. The last 2 cases have to be last and in that order, as other types would be caught by them
					switch (classProperty)
					{
						case bool x:
							CreateBooleanCapability(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, orderIndex, x);
							break;

						case Domain x:
							CreateDomainCapability(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, orderIndex, x);
							break;

						case int x:
							CreateNumberCapability(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, orderIndex, x);
							break;

						case long x:
							CreateLongCapability(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, orderIndex, x);
							break;

						case NIST.CVP.Algorithms.DataTypes.Range x:
							CreateRangeCapability(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, orderIndex, x);
							break;

						case string x:
							CreateStringCapability(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, orderIndex, x);
							break;

						case List<bool> x:
							CreateBooleanArrayCapability(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, orderIndex, x);
							break;

						case List<int> x:
							CreateNumberArrayCapability(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, orderIndex, x);
							break;

						case List<long> x:
							CreateLongArrayCapability(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, orderIndex, x);
							break;

						case List<NIST.CVP.Algorithms.DataTypes.Range> x:
							CreateRangeArrayCapability(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, orderIndex, x);
							break;

						case List<string> x:
							CreateStringArrayCapability(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, orderIndex, x);
							break;

						case IList x:
							CreateCompositeArrayCapability(algorithmID, scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, orderIndex, propertyName, x);
							break;

						case object x:
							CreateCompositeCapability(algorithmID, scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, orderIndex, propertyName, x);
							break;
					};
				}
			}
		}

		public void CreateBooleanCapability(long scenarioAlgorithmID, long propertyID, long? rootCapabilityID, long? parentCapabilityID, int level, int orderIndex, bool value)
		{
			_capabilityProvider.Insert(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, AlgorithmPropertyType.Boolean, orderIndex, false, null, null, value);
		}

		public void CreateLongCapability(long scenarioAlgorithmID, long propertyID, long? rootCapabilityID, long? parentCapabilityID, int level, int orderIndex, long value)
		{
			_capabilityProvider.Insert(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, AlgorithmPropertyType.Long, orderIndex, false, null, value, null);
		}

		public void CreateNumberCapability(long scenarioAlgorithmID, long propertyID, long? rootCapabilityID, long? parentCapabilityID, int level, int orderIndex, int value)
		{
			_capabilityProvider.Insert(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, AlgorithmPropertyType.Number, orderIndex, false, null, value, null);
		}

		public void CreateStringCapability(long scenarioAlgorithmID, long propertyID, long? rootCapabilityID, long? parentCapabilityID, int level, int orderIndex, string value)
		{
			_capabilityProvider.Insert(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, AlgorithmPropertyType.String, orderIndex, false, value, null, null);
		}

		public void CreateRangeCapability(long scenarioAlgorithmID, long propertyID, long? rootCapabilityID, long? parentCapabilityID, int level, int orderIndex, NIST.CVP.Algorithms.DataTypes.Range value)
		{
			string stringValue = JsonSerializer.Serialize(value, new JsonSerializerOptions { IgnoreNullValues = true });
			_capabilityProvider.Insert(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, AlgorithmPropertyType.Range, orderIndex, false, stringValue, null, null);
		}

		public void CreateDomainCapability(long scenarioAlgorithmID, long propertyID, long? rootCapabilityID, long? parentCapabilityID, int level, int orderIndex, Domain value)
		{
			//Need to turn the segments into a string that is a JSON array of numbers and Range objects. This could probably be done somehow in the DomainConverter, but just doing it here for now 
			List<string> segmentsAsStrings = new List<string>();

			foreach (var segment in value.Segments)
			{
				if (segment is NumericSegment) { segmentsAsStrings.Add(((NumericSegment)segment).Value.ToString()); }
				else { segmentsAsStrings.Add(JsonSerializer.Serialize((NIST.CVP.Algorithms.DataTypes.Range)segment, new JsonSerializerOptions { IgnoreNullValues = true })); }
			}

			//Join the collection with a comma delimiter and put brackets around it => looks like a JSON array
			string stringValue = $"[{String.Join(",", segmentsAsStrings)}]";

			_capabilityProvider.Insert(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, AlgorithmPropertyType.Domain, orderIndex, false, stringValue, null, null);
		}

		public void CreateCompositeCapability(long algorithmID, long scenarioAlgorithmID, long propertyID, long? rootCapabilityID, long? parentCapabilityID, int level, int orderIndex, string propertyName, object value)
		{
			//Create the container capability
			InsertResult result = _capabilityProvider.Insert(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, AlgorithmPropertyType.Composite, orderIndex, false, null, null, null);

			//Grab the ID of the container that was just created, it will be the parent of the child capabilities
			long containerCapabilityID = result.ID;

			//If this object was a level 0 object, then the root for the children is the container
			if (level == 0) rootCapabilityID = containerCapabilityID;

			//Create the child capabilities from this object
			CreateClassCapabilities(algorithmID, scenarioAlgorithmID, rootCapabilityID, containerCapabilityID, level + 1, orderIndex, propertyName, value);
		}


		public void CreateBooleanArrayCapability(long scenarioAlgorithmID, long propertyID, long? rootCapabilityID, long? parentCapabilityID, int level, int orderIndex, List<bool> values)
		{
			//Don't do anything if the collection is empty
			if (values.Count == 0) return;

			//Create the container capability
			InsertResult result = _capabilityProvider.Insert(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, AlgorithmPropertyType.BooleanArray, orderIndex, false, null, null, null);

			//Grab the ID of the container that was just created, it will be the parent of the value capabilities
			long containerCapabilityID = result.ID;

			//If this object was a level 0 object, then the root for the children is the container
			if (level == 0) rootCapabilityID = containerCapabilityID;

			//Create the value capabilities
			for (int i = 0; i < values.Count; i++)
			{
				CreateBooleanCapability(scenarioAlgorithmID, propertyID, rootCapabilityID, containerCapabilityID, level + 1, i, values[i]);
			}
		}

		public void CreateLongArrayCapability(long scenarioAlgorithmID, long propertyID, long? rootCapabilityID, long? parentCapabilityID, int level, int orderIndex, List<long> values)
		{
			//Don't do anything if the collection is empty
			if (values.Count == 0) return;

			//Create the container capability
			InsertResult result = _capabilityProvider.Insert(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, AlgorithmPropertyType.LongArray, orderIndex, false, null, null, null);

			//Grab the ID of the container that was just created, it will be the parent of the value capabilities
			long containerCapabilityID = result.ID;

			//If this object was a level 0 object, then the root for the children is the container
			if (level == 0) rootCapabilityID = containerCapabilityID;

			//Create the value capabilities
			for (int i = 0; i < values.Count; i++)
			{
				CreateLongCapability(scenarioAlgorithmID, propertyID, rootCapabilityID, containerCapabilityID, level + 1, i, values[i]);
			}
		}

		public void CreateNumberArrayCapability(long scenarioAlgorithmID, long propertyID, long? rootCapabilityID, long? parentCapabilityID, int level, int orderIndex, List<int> values)
		{
			//Don't do anything if the collection is empty
			if (values.Count == 0) return;

			//Create the container capability
			InsertResult result = _capabilityProvider.Insert(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, AlgorithmPropertyType.NumberArray, orderIndex, false, null, null, null);

			//Grab the ID of the container that was just created, it will be the parent of the value capabilities
			long containerCapabilityID = result.ID;

			//If this object was a level 0 object, then the root for the children is the container
			if (level == 0) rootCapabilityID = containerCapabilityID;

			//Create the value capabilities
			for (int i = 0; i < values.Count; i++)
			{
				CreateNumberCapability(scenarioAlgorithmID, propertyID, rootCapabilityID, containerCapabilityID, level + 1, i, values[i]);
			}
		}

		public void CreateStringArrayCapability(long scenarioAlgorithmID, long propertyID, long? rootCapabilityID, long? parentCapabilityID, int level, int orderIndex, List<string> values)
		{
			//Don't do anything if the collection is empty
			if (values.Count == 0) return;

			//Create the container capability
			InsertResult result = _capabilityProvider.Insert(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, AlgorithmPropertyType.StringArray, orderIndex, false, null, null, null);

			//Grab the ID of the container that was just created, it will be the parent of the value capabilities
			long containerCapabilityID = result.ID;

			//If this object was a level 0 object, then the root for the children is the container
			if (level == 0) rootCapabilityID = containerCapabilityID;

			//Create the value capabilities
			for (int i = 0; i < values.Count; i++)
			{
				CreateStringCapability(scenarioAlgorithmID, propertyID, rootCapabilityID, containerCapabilityID, level + 1, i, values[i]);
			}
		}

		public void CreateRangeArrayCapability(long scenarioAlgorithmID, long propertyID, long? rootCapabilityID, long? parentCapabilityID, int level, int orderIndex, List<NIST.CVP.Algorithms.DataTypes.Range> values)
		{
			//Don't do anything if the collection is empty
			if (values.Count == 0) return;

			//Create the container capability
			InsertResult result = _capabilityProvider.Insert(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, AlgorithmPropertyType.RangeArray, orderIndex, false, null, null, null);

			//Grab the ID of the container that was just created, it will be the parent of the value capabilities
			long containerCapabilityID = result.ID;

			//If this object was a level 0 object, then the root for the children is the container
			if (level == 0) rootCapabilityID = containerCapabilityID;

			//Create the child range capabilities
			for (int i = 0; i < values.Count; i++)
			{
				CreateRangeCapability(scenarioAlgorithmID, propertyID, rootCapabilityID, containerCapabilityID, level + 1, i, values[i]);
			}
		}

		public void CreateCompositeArrayCapability(long algorithmID, long scenarioAlgorithmID, long propertyID, long? rootCapabilityID, long? parentCapabilityID, int level, int orderIndex, string propertyName, IList values)
		{
			//Don't do anything if the collection is empty
			if (values.Count == 0) return;

			//Create the container capability
			InsertResult result = _capabilityProvider.Insert(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, AlgorithmPropertyType.CompositeArray, orderIndex, false, null, null, null);

			//Grab the ID of the container that was just created, it will be the parent of the value capabilities
			long containerCapabilityID = result.ID;

			//If this object was a level 0 object, then the root for the children is the container
			if (level == 0) rootCapabilityID = containerCapabilityID;

			//Create the child composite capabilities
			for (int i = 0; i < values.Count; i++)
			{
				CreateCompositeCapability(algorithmID, scenarioAlgorithmID, propertyID, rootCapabilityID, containerCapabilityID, level + 1, i, propertyName, values[i]);
			}
		}
	}
}
