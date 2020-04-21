using NIST.CVP.Libraries.Shared.Results;
using NIST.CVP.Libraries.Internal.LCAVPCore.Registration;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.Processors
{
	public interface IPersonProcessor
	{
		InsertResult Create(Contact person);
		void Update(Contact person);
	}
}
