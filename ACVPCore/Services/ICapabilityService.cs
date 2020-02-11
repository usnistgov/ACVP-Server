using System;

namespace ACVPCore.Services
{
	public interface ICapabilityService
	{
		void CreateClassCapabilities(long algorithmID, long scenarioAlgorithmID, long? rootCapabilityID, long? parentCapabilityID, int level, int orderIndex, string parentPropertyName, Object objectClass);
	}
}