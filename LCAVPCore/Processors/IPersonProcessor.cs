using ACVPCore.Results;
using LCAVPCore.Registration;

namespace LCAVPCore.Processors
{
	public interface IPersonProcessor
	{
		InsertResult Create(Contact person);
		void Update(Contact person);
	}
}
