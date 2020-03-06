using System.Collections.Generic;
using ACVPCore.Models;

namespace ACVPCore.Providers
{
	public interface IPropertyProvider
	{
		List<PropertyLookup> GetProperties();
	}
}