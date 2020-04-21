using System;
using System.Collections.Generic;
using NIST.CVP.Libraries.Internal.ACVPCore.Models;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Services
{
	public interface IPropertyService
	{
		PropertyLookup LookupProperty(long algorithmID, string propertyName);
		List<string> VerifyAlgorithmModels();
		List<string> GetAlgorithmPropertyTree(string algorithmClassName);
	}
}
