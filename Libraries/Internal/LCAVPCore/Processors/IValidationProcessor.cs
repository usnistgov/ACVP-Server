using NIST.CVP.Results;
using LCAVPCore.Registration;

namespace LCAVPCore.Processors
{
	public interface IValidationProcessor
	{
		InsertResult Create(NewRegistrationContainer module);
		void Update(UpdateRegistrationContainer module);
	}
}
