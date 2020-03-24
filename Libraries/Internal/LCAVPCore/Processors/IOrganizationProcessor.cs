using NIST.CVP.Results;
using LCAVPCore.Registration;

namespace LCAVPCore.Processors
{
	public interface IOrganizationProcessor
	{
		InsertResult Create(Vendor organization);
		void Update(Vendor organization);
	}
}
