using ACVPCore.Results;
using LCAVPCore.Registration;

namespace LCAVPCore.Processors
{
	public interface IOEProcessor
	{
		InsertResult Create(OperationalEnvironment oe);
		void Update(OperationalEnvironment oe);
	}
}
