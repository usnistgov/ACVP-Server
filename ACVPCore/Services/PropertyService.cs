using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ACVPCore.Algorithms.Persisted;
using ACVPCore.Models;
using ACVPCore.Providers;

namespace ACVPCore.Services
{
	public class PropertyService : IPropertyService
	{
		IPropertyProvider _propertyProvider;
		IAlgorithmService _algorithmService;
		private static Hashtable _properties = new Hashtable();         //Optimize this to be a hashtable? Key would be algorithmID|PropertyName

		public PropertyService(IPropertyProvider propertyProvider, IAlgorithmService algorithmService)
		{
			_propertyProvider = propertyProvider;
			_algorithmService = algorithmService;

			//Get all the properties, add them all to the hash table for quick lookup. Keeping all the properties in the value just to be lazy/safe
			_propertyProvider.GetProperties().ForEach(x => _properties.Add($"{x.AlgorithmID}|{x.Name}", x));
		}

		public PropertyLookup LookupProperty(long algorithmID, string propertyName) => (PropertyLookup)_properties[$"{algorithmID}|{propertyName}"];

		public List<string> VerifyAlgorithmModels()
		{
			List<string> results = new List<string>();

			//Get all the algorithm Types (classes) that extend PersistedAlgorithmBase
			var baseType = typeof(PersistedAlgorithmBase);
			var assembly = baseType.Assembly;
			var persistedAlgorithmTypes = assembly.GetTypes().Where(t => t.IsSubclassOf(baseType));

			//Loop through each type and compare the class to what the database says it should look like
			foreach (Type persistedAlgorithmType in persistedAlgorithmTypes)
			{
				List<string> errors = new List<string>();

				//Instantiate this algorithm because we need to get the name/mode/revision, but really only need it as a base
				var algorithm = (PersistedAlgorithmBase)Activator.CreateInstance(persistedAlgorithmType);

				//Get all the properties for this algorithm based on the Persisted class
				List<PropertyTreeValidationNode> fromModel = GetPropertyTreeFromModel(persistedAlgorithmType, 0, null);

				//Get the algorithm ID
				long algorithmID = _algorithmService.LookupAlgorithm(algorithm.Name, algorithm.Mode, algorithm.Revision).AlgorithmID;

				//Get all the properties for this algorithm from the database
				List<PropertyTreeValidationNode> fromDatabase = _propertyProvider.GetPropertyTreeValidationNodes(algorithmID);

				//Make sure that the same number of properties are in each. If not, know something is wrong
				if (fromModel.Count != fromDatabase.Count)
				{
					errors.Add($"{algorithm.Name}|{algorithm.Mode}|{algorithm.Revision}|{algorithmID} property count mismatch. Model: {fromModel.Count} Database: {fromDatabase.Count}");
				}
				else
				{
					//Loop through those two lists and compare. Every property should match.
					for (int i = 0; i < fromModel.Count; i++)
					{
						if (fromModel[i].Name != fromDatabase[i].Name)
						{
							errors.Add($"{algorithm.Name}|{algorithm.Mode}|{algorithm.Revision}|{algorithmID} property name mismatch. Model: {fromModel[i].Name} Database: {fromDatabase[i].Name}");
						}
					}
				}

				if (errors.Count > 0)
				{
					results.AddRange(errors);
				}
				else
				{
					results.Add($"{algorithm.Name}|{algorithm.Mode}|{algorithm.Revision}|{algorithmID} verified");
				}
			}

			return results;
		}

		public List<string> GetAlgorithmPropertyTree(string algorithmClassName)
		{
			Type baseType = typeof(PersistedAlgorithmBase);
			Assembly assembly = baseType.Assembly;

			string qualifiedClassName = "ACVPCore.Algorithms.Persisted." + algorithmClassName;

			Type algorithmType = assembly.GetType(qualifiedClassName);

			return GetPropertyTreeFromModel(algorithmType, 0, null).Select(x => $"{new String(' ', x.Level * 4)}{x.Name}").ToList();
		}

		private List<PropertyTreeValidationNode> GetPropertyTreeFromModel(Type t, int level, string parentPropertyName)
		{
			List<PropertyTreeValidationNode> nodes = new List<PropertyTreeValidationNode>();

			//Using reflection get all the (C#) properties of this type, then filter down to only the ones we've annotated to be (ACVP) properties, then sort them by their name (since we don't track order index yet)
			List<(PropertyInfo prop, AlgorithmProperty algorithmProperty)> propertyList = t.GetProperties().Select(x => (x, (AlgorithmProperty)Attribute.GetCustomAttribute(x, typeof(AlgorithmProperty)))).Where(z => z.Item2 != null).OrderBy(x => x.Item2.Name).ToList();

			foreach (var property in propertyList)
			{
				string propertyName = property.algorithmProperty.PrependParentPropertyName ? $"{parentPropertyName}/{property.algorithmProperty.Name}" : property.algorithmProperty.Name;
				nodes.Add(new PropertyTreeValidationNode
				{
					Name = propertyName,
					Level = level
				});

				//If the property is a List, need to determine the type of class it is a list of, so we can then recurse down into that type - treating it as a list gets you nowhere
				if (property.prop.PropertyType.IsGenericType && property.prop.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
				{
					nodes.AddRange(GetPropertyTreeFromModel(property.prop.PropertyType.GetGenericArguments().FirstOrDefault(), level + 1, propertyName));
				}
				else
				{
					//Just a singular type, so just make the basic recursive call
					nodes.AddRange(GetPropertyTreeFromModel(property.prop.PropertyType, level + 1, propertyName));
				}
			}

			return nodes;
		}
	}
}
