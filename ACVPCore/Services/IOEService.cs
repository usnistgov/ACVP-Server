using ACVPCore.Models.Parameters;
using ACVPCore.Results;

namespace ACVPCore.Services
{
	public interface IOEService
	{
		OEResult Create(OECreateParameters oe);
		OEResult Update(OEUpdateParameters parameters);
		DeleteResult Delete(long oeID);
		bool OEIsUsed(long oeID);
	}
}