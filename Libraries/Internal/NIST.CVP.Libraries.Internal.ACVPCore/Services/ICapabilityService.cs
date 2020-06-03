using System;
using NIST.CVP.Libraries.Shared.Results;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Services
{
	public interface ICapabilityService
	{
		Result DeleteAllForValidationOEAlgorithm(long validationOEAlgorithmID);
		void CreateClassCapabilities(long algorithmID, long valdiationOEAlgorithmID, long? parentCapabilityID, int orderIndex, string parentPropertyName, Object objectClass);
	}
}