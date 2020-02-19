using ACVPCore.Models.Parameters;
using ACVPCore.Results;

namespace ACVPCore.Services
{
	public interface IPersonService
	{
		PersonResult Create(PersonCreateParameters parameters);
		DeleteResult Delete(long personID);
		bool PersonIsUsed(long personID);
		bool PersonExists(long personID);
		PersonResult Update(PersonUpdateParameters parameters);
	}
}