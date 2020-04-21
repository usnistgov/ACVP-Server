using NIST.CVP.Libraries.Internal.LCAVPCore.Registration;
using NIST.CVP.Libraries.Shared.Results;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.Processors
{
	public interface IModuleProcessor
	{
		InsertResult Create(Module module);
		void Update(Module module);
	}
}
