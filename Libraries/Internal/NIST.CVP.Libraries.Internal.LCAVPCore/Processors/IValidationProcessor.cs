using NIST.CVP.Libraries.Shared.Results;
using NIST.CVP.Libraries.Internal.LCAVPCore.Registration;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.Processors
{
	public interface IValidationProcessor
	{
		InsertResult Create(NewRegistrationContainer module);
		void Update(UpdateRegistrationContainer module);
	}
}
