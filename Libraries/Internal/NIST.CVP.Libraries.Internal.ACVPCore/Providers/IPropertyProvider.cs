using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Providers
{
	public interface IPropertyProvider
	{
		List<PropertyLookup> GetProperties();
		List<PropertyTreeValidationNode> GetPropertyTreeValidationNodes(long algorithmID);
	}
}