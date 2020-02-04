using ACVPCore.Models;

namespace ACVPCore.Services
{
	public interface IPropertyService
	{
		PropertyLookup LookupProperty(long algorithmID, string propertyName);
	}
}
