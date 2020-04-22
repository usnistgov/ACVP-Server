using NIST.CVP.Libraries.Shared.Results;
using NIST.CVP.Libraries.Internal.LCAVPCore.Registration;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.Processors
{
	public interface IOrganizationProcessor
	{
		InsertResult Create(Vendor organization);
		void Update(Vendor organization);
	}
}
