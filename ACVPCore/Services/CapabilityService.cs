using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using ACVPCore.Algorithms.DataTypes;
using ACVPCore.Algorithms.Persisted;
using ACVPCore.Models;
using ACVPCore.Providers;
using ACVPCore.Results;

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

		public void CreateClassCapabilities(long algorithmID, long scenarioAlgorithmID, long? rootCapabilityID, long? parentCapabilityID, int level, int orderIndex, Object objectClass)
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
							CreateBooleanCapability(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, orderIndex, (bool)classProperty);
							break;

						case AlgorithmPropertyType.BooleanArray:
							CreateBooleanArrayCapability(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, orderIndex, (List<bool>)classProperty);
							break;

						case AlgorithmPropertyType.Composite:
							CreateCompositeCapability(algorithmID, scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, orderIndex, classProperty);
							break;

						case AlgorithmPropertyType.CompositeArray:
							CreateCompositeArrayCapability(algorithmID, scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, orderIndex, (List<object>)classProperty);
							break;

						case AlgorithmPropertyType.Domain:
							CreateDomainCapability(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, orderIndex, (Domain)classProperty);
							break;

						case AlgorithmPropertyType.Long:
							CreateLongCapability(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, orderIndex, (long)classProperty);
							break;

						case AlgorithmPropertyType.LongArray:
							CreateLongArrayCapability(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, orderIndex, (List<long>)classProperty);
							break;

						case AlgorithmPropertyType.Number:
							CreateNumberCapability(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, orderIndex, (long)classProperty);
							break;

						case AlgorithmPropertyType.NumberArray:
							CreateNumberArrayCapability(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, orderIndex, (List<long>)classProperty);
							break;

						case AlgorithmPropertyType.Range:
							CreateRangeCapability(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, orderIndex, (Algorithms.DataTypes.Range)classProperty);
							break;

						case AlgorithmPropertyType.RangeArray:
							CreateRangeArrayCapability(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, orderIndex, (List<Algorithms.DataTypes.Range>)classProperty);
							break;

						case AlgorithmPropertyType.String:
							CreateStringCapability(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, orderIndex, (string)classProperty);
							break;

						case AlgorithmPropertyType.StringArray:
							CreateStringArrayCapability(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, orderIndex, (List<string>)classProperty);
							break;
					}
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

		public void CreateNumberCapability(long scenarioAlgorithmID, long propertyID, long? rootCapabilityID, long? parentCapabilityID, int level, int orderIndex, long value)
		{
			_capabilityProvider.Insert(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, AlgorithmPropertyType.Number, orderIndex, false, null, value, null);
		}

		public void CreateStringCapability(long scenarioAlgorithmID, long propertyID, long? rootCapabilityID, long? parentCapabilityID, int level, int orderIndex, string value)
		{
			_capabilityProvider.Insert(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, AlgorithmPropertyType.String, orderIndex, false, value, null, null);
		}

		public void CreateRangeCapability(long scenarioAlgorithmID, long propertyID, long? rootCapabilityID, long? parentCapabilityID, int level, int orderIndex, ACVPCore.Algorithms.DataTypes.Range value)
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
				else { segmentsAsStrings.Add(JsonSerializer.Serialize((Algorithms.DataTypes.Range)segment, new JsonSerializerOptions { IgnoreNullValues = true })); }
			}

			//Join the collection with a comma delimiter and put brackets around it => looks like a JSON array
			string stringValue = $"[{String.Join(",", segmentsAsStrings)}]";

			_capabilityProvider.Insert(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, AlgorithmPropertyType.Domain, orderIndex, false, stringValue, null, null);
		}

		public void CreateCompositeCapability(long algorithmID, long scenarioAlgorithmID, long propertyID, long? rootCapabilityID, long? parentCapabilityID, int level, int orderIndex, object value)
		{
			//Create the container capability
			InsertResult result = _capabilityProvider.Insert(scenarioAlgorithmID, propertyID, rootCapabilityID, parentCapabilityID, level, AlgorithmPropertyType.Composite, orderIndex, false, null, null, null);

			//Grab the ID of the container that was just created, it will be the parent of the child capabilities
			long containerCapabilityID = result.ID;

			//If this object was a level 0 object, then the root for the children is the container
			if (level == 0) rootCapabilityID = containerCapabilityID;

			//Create the child capabilities from this object
			CreateClassCapabilities(algorithmID, scenarioAlgorithmID, rootCapabilityID, containerCapabilityID, level + 1, orderIndex, value);
		}


		public void CreateBooleanArrayCapability(long scenarioAlgorithmID, long propertyID, long? rootCapabilityID, long? parentCapabilityID, int level, int orderIndex, List<bool> values)
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
				CreateBooleanCapability(scenarioAlgorithmID, propertyID, rootCapabilityID, containerCapabilityID, level + 1, i, values[i]);
			}
		}

		public void CreateLongArrayCapability(long scenarioAlgorithmID, long propertyID, long? rootCapabilityID, long? parentCapabilityID, int level, int orderIndex, List<long> values)
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
				CreateLongCapability(scenarioAlgorithmID, propertyID, rootCapabilityID, containerCapabilityID, level + 1, i, values[i]);
			}
		}

		public void CreateNumberArrayCapability(long scenarioAlgorithmID, long propertyID, long? rootCapabilityID, long? parentCapabilityID, int level, int orderIndex, List<long> values)
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
				CreateNumberCapability(scenarioAlgorithmID, propertyID, rootCapabilityID, containerCapabilityID, level + 1, i, values[i]);
			}
		}

		public void CreateStringArrayCapability(long scenarioAlgorithmID, long propertyID, long? rootCapabilityID, long? parentCapabilityID, int level, int orderIndex, List<string> values)
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
				CreateStringCapability(scenarioAlgorithmID, propertyID, rootCapabilityID, containerCapabilityID, level + 1, i, values[i]);
			}
		}

		public void CreateRangeArrayCapability(long scenarioAlgorithmID, long propertyID, long? rootCapabilityID, long? parentCapabilityID, int level, int orderIndex, List<ACVPCore.Algorithms.DataTypes.Range> values)
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
				CreateRangeCapability(scenarioAlgorithmID, propertyID, rootCapabilityID, containerCapabilityID, level + 1, i, values[i]);
			}
		}

		public void CreateCompositeArrayCapability(long algorithmID, long scenarioAlgorithmID, long propertyID, long? rootCapabilityID, long? parentCapabilityID, int level, int orderIndex, List<object> values)
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
				CreateCompositeCapability(algorithmID, scenarioAlgorithmID, propertyID, rootCapabilityID, containerCapabilityID, level + 1, i, values[i]);
			}
		}
	}
}
