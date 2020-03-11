using System;
using System.Collections.Generic;
using ACVPCore.Models;

namespace ACVPCore.Services
{
	public interface IPropertyService
	{
		PropertyLookup LookupProperty(long algorithmID, string propertyName);
		List<string> VerifyAlgorithmModels();
		List<string> GetAlgorithmPropertyTree(string algorithmClassName);
	}
}
