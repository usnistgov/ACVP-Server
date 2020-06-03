using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using NIST.CVP.Libraries.Internal.ACVPCore.Providers;
using NIST.CVP.Libraries.Internal.Algorithms.Persisted;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models;
using NIST.CVP.Libraries.Shared.Algorithms.DataTypes;
using NIST.CVP.Libraries.Shared.Results;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Services
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

		public Result DeleteAllForValidationOEAlgorithm(long validationOEAlgorithmID) => _capabilityProvider.DeleteAllForValidationOEAlgorithm(validationOEAlgorithmID);

		public void CreateClassCapabilities(long algorithmID, long validationOEAlgorithmID, long? parentCapabilityID, int orderIndex, string parentPropertyName, Object objectClass)
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
							CreateBooleanCapability(validationOEAlgorithmID, propertyID, parentCapabilityID, orderIndex, x);
							break;

						case Domain x:
							CreateDomainCapability(validationOEAlgorithmID, propertyID, parentCapabilityID, orderIndex, x);
							break;

						case int x:
							CreateNumericCapability(validationOEAlgorithmID, propertyID, parentCapabilityID, orderIndex, x);
							break;

						case long x:
							CreateNumericCapability(validationOEAlgorithmID, propertyID, parentCapabilityID, orderIndex, x);
							break;

						case Shared.Algorithms.DataTypes.Range x:
							CreateRangeCapability(validationOEAlgorithmID, propertyID, parentCapabilityID, orderIndex, x);
							break;

						case string x:
							CreateStringCapability(validationOEAlgorithmID, propertyID, parentCapabilityID, orderIndex, x);
							break;

						case List<bool> x:
							CreateBooleanArrayCapability(validationOEAlgorithmID, propertyID, parentCapabilityID, orderIndex, x);
							break;

						case List<int> x:
							CreateNumberArrayCapability(validationOEAlgorithmID, propertyID, parentCapabilityID, orderIndex, x);
							break;

						case List<long> x:
							CreateLongArrayCapability(validationOEAlgorithmID, propertyID, parentCapabilityID, orderIndex, x);
							break;

						case List<Shared.Algorithms.DataTypes.Range> x:
							CreateRangeArrayCapability(validationOEAlgorithmID, propertyID, parentCapabilityID, orderIndex, x);
							break;

						case List<string> x:
							CreateStringArrayCapability(validationOEAlgorithmID, propertyID, parentCapabilityID, orderIndex, x);
							break;

						case IList x:
							CreateCompositeArrayCapability(algorithmID, validationOEAlgorithmID, propertyID, parentCapabilityID, orderIndex, propertyName, x);
							break;

						case object x:
							CreateCompositeCapability(algorithmID, validationOEAlgorithmID, propertyID, parentCapabilityID, orderIndex, propertyName, x);
							break;
					};
				}
			}
		}

		public void CreateBooleanCapability(long validationOEAlgorithmID, long propertyID, long? parentCapabilityID, int orderIndex, bool value)
		{
			_capabilityProvider.Insert(validationOEAlgorithmID, propertyID, parentCapabilityID, orderIndex, false, null, null, value);
		}

		public void CreateNumericCapability(long validationOEAlgorithmID, long propertyID, long? parentCapabilityID, int orderIndex, long value)
		{
			_capabilityProvider.Insert(validationOEAlgorithmID, propertyID,  parentCapabilityID, orderIndex, false, null, value, null);
		}

		public void CreateStringCapability(long validationOEAlgorithmID, long propertyID, long? parentCapabilityID, int orderIndex, string value)
		{
			_capabilityProvider.Insert(validationOEAlgorithmID, propertyID, parentCapabilityID, orderIndex, false, value, null, null);
		}

		public void CreateRangeCapability(long validationOEAlgorithmID, long propertyID, long? parentCapabilityID, int orderIndex, Shared.Algorithms.DataTypes.Range value)
		{
			string stringValue = JsonSerializer.Serialize(value, new JsonSerializerOptions { IgnoreNullValues = true });
			_capabilityProvider.Insert(validationOEAlgorithmID, propertyID, parentCapabilityID, orderIndex, false, stringValue, null, null);
		}

		public void CreateDomainCapability(long validationOEAlgorithmID, long propertyID, long? parentCapabilityID, int orderIndex, Domain value)
		{
			//Need to turn the segments into a string that is a JSON array of numbers and Range objects. This could probably be done somehow in the DomainConverter, but just doing it here for now 
			List<string> segmentsAsStrings = new List<string>();

			foreach (var segment in value.Segments)
			{
				if (segment is NumericSegment) { segmentsAsStrings.Add(((NumericSegment)segment).Value.ToString()); }
				else { segmentsAsStrings.Add(JsonSerializer.Serialize((Shared.Algorithms.DataTypes.Range)segment, new JsonSerializerOptions { IgnoreNullValues = true })); }
			}

			//Join the collection with a comma delimiter and put brackets around it => looks like a JSON array
			string stringValue = $"[{String.Join(",", segmentsAsStrings)}]";

			_capabilityProvider.Insert(validationOEAlgorithmID, propertyID, parentCapabilityID, orderIndex, false, stringValue, null, null);
		}

		public void CreateCompositeCapability(long algorithmID, long validationOEAlgorithmID, long propertyID, long? parentCapabilityID, int orderIndex, string propertyName, object value)
		{
			//Create the container capability, ID will be the parent of the child capabilities
			long containerCapabilityID = CreateContainerCapability(validationOEAlgorithmID, propertyID, parentCapabilityID, orderIndex);

			//Create the child capabilities from this object
			CreateClassCapabilities(algorithmID, validationOEAlgorithmID, containerCapabilityID, orderIndex, propertyName, value);
		}

		public void CreateBooleanArrayCapability(long validationOEAlgorithmID, long propertyID, long? parentCapabilityID, int orderIndex, List<bool> values)
		{
			//Don't do anything if the collection is empty
			if (values.Count == 0) return;

			//Create the container capability, ID will be the parent of the child capabilities
			long containerCapabilityID = CreateContainerCapability(validationOEAlgorithmID, propertyID, parentCapabilityID, orderIndex);

			//Create the value capabilities
			for (int i = 0; i < values.Count; i++)
			{
				CreateBooleanCapability(validationOEAlgorithmID, propertyID, containerCapabilityID, i, values[i]);
			}
		}

		public void CreateLongArrayCapability(long validationOEAlgorithmID, long propertyID, long? parentCapabilityID, int orderIndex, List<long> values)
		{
			//Don't do anything if the collection is empty
			if (values.Count == 0) return;

			//Create the container capability, ID will be the parent of the child capabilities
			long containerCapabilityID = CreateContainerCapability(validationOEAlgorithmID, propertyID, parentCapabilityID, orderIndex);

			//Create the value capabilities
			for (int i = 0; i < values.Count; i++)
			{
				CreateNumericCapability(validationOEAlgorithmID, propertyID, containerCapabilityID, i, values[i]);
			}
		}

		public void CreateNumberArrayCapability(long validationOEAlgorithmID, long propertyID, long? parentCapabilityID, int orderIndex, List<int> values)
		{
			//Don't do anything if the collection is empty
			if (values.Count == 0) return;

			//Create the container capability, ID will be the parent of the child capabilities
			long containerCapabilityID = CreateContainerCapability(validationOEAlgorithmID, propertyID, parentCapabilityID, orderIndex);

			//Create the value capabilities
			for (int i = 0; i < values.Count; i++)
			{
				CreateNumericCapability(validationOEAlgorithmID, propertyID, containerCapabilityID, i, values[i]);
			}
		}

		public void CreateStringArrayCapability(long validationOEAlgorithmID, long propertyID, long? parentCapabilityID, int orderIndex, List<string> values)
		{
			//Don't do anything if the collection is empty
			if (values.Count == 0) return;

			//Create the container capability, ID will be the parent of the child capabilities
			long containerCapabilityID = CreateContainerCapability(validationOEAlgorithmID, propertyID, parentCapabilityID, orderIndex);

			//Create the value capabilities
			for (int i = 0; i < values.Count; i++)
			{
				CreateStringCapability(validationOEAlgorithmID, propertyID, containerCapabilityID, i, values[i]);
			}
		}

		public void CreateRangeArrayCapability(long validationOEAlgorithmID, long propertyID, long? parentCapabilityID, int orderIndex, List<Shared.Algorithms.DataTypes.Range> values)
		{
			//Don't do anything if the collection is empty
			if (values.Count == 0) return;

			//Create the container capability, ID will be the parent of the child capabilities
			long containerCapabilityID = CreateContainerCapability(validationOEAlgorithmID, propertyID, parentCapabilityID, orderIndex);

			//Create the child range capabilities
			for (int i = 0; i < values.Count; i++)
			{
				CreateRangeCapability(validationOEAlgorithmID, propertyID, containerCapabilityID, i, values[i]);
			}
		}

		public void CreateCompositeArrayCapability(long algorithmID, long validationOEAlgorithmID, long propertyID, long? parentCapabilityID, int orderIndex, string propertyName, IList values)
		{
			//Don't do anything if the collection is empty
			if (values.Count == 0) return;

			//Create the container capability, ID will be the parent of the child capabilities
			long containerCapabilityID = CreateContainerCapability(validationOEAlgorithmID, propertyID, parentCapabilityID, orderIndex);

			//Create the child composite capabilities
			for (int i = 0; i < values.Count; i++)
			{
				CreateCompositeCapability(algorithmID, validationOEAlgorithmID, propertyID, containerCapabilityID, i, propertyName, values[i]);
			}
		}

		private long CreateContainerCapability(long validationOEAlgorithmID, long propertyID, long? parentCapabilityID, int orderIndex)
		{
			InsertResult result = _capabilityProvider.Insert(validationOEAlgorithmID, propertyID, parentCapabilityID, orderIndex, false, null, null, null);

			//Grab the ID of the container that was just created, it will be the parent of the value capabilities
			return result.ID;
		}
	}
}
