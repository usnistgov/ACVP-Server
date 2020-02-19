using ACVPCore.Models;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;

namespace ACVPCore.Services
{
	public interface IPersonService
	{
		Person Get(long personID);
		PersonResult Create(PersonCreateParameters parameters);
		DeleteResult Delete(long personID);
		bool PersonIsUsed(long personID);
		PersonResult Update(PersonUpdateParameters parameters);
	}
}