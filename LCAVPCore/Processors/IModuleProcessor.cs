using ACVPCore.Results;
using LCAVPCore.Registration;

namespace LCAVPCore.Processors
{
	public interface IModuleProcessor
	{
		InsertResult Create(Module module);
		void Update(Module module);
	}
}
