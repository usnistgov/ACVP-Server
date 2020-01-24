using System.Collections;
using ACVPCore.Models;
using ACVPCore.Providers;

namespace ACVPCore.Services
{
	public class PropertyService : IPropertyService
	{
		IPropertyProvider _propertyProvider;
		private static Hashtable _properties = new Hashtable();			//Optimize this to be a hashtable? Key would be algorithmID|PropertyName

		public PropertyService(IPropertyProvider propertyProvider)
		{
			_propertyProvider = propertyProvider;

			//Get all the properties, add them all to the hash table for quick lookup. Keeping all the properties in the value just to be lazy/safe
			_propertyProvider.GetProperties().ForEach(x => _properties.Add($"{x.AlgorithmID}!{x.Name}", x));
		}

		public PropertyLookup LookupProperty(long algorithmID, string propertyName) => (PropertyLookup)_properties[$"{algorithmID}|{propertyName}"];
	}
}
