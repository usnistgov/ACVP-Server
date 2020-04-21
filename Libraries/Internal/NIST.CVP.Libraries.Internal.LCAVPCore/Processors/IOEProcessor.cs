using NIST.CVP.Libraries.Shared.Results;
using NIST.CVP.Libraries.Internal.LCAVPCore.Registration;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.Processors
{
	public interface IOEProcessor
	{
		InsertResult Create(OperationalEnvironment oe);
		void Update(OperationalEnvironment oe);
	}
}
