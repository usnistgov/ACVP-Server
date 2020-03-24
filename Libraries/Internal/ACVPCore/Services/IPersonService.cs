using ACVPCore.Models;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;
using NIST.CVP.Enumerables;
using NIST.CVP.Results;

using System.Collections.Generic;

namespace ACVPCore.Services
{
	public interface IPersonService
	{
		Person Get(long personID);
		PersonResult Create(PersonCreateParameters parameters);
		DeleteResult Delete(long personID);
		bool PersonIsUsed(long personID);
		bool PersonExists(long personID);
		PersonResult Update(PersonUpdateParameters parameters);
		PagedEnumerable<PersonLite> Get(PersonListParameters param);
	}
}