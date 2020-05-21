using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Results;
using NIST.CVP.Libraries.Shared.Enumerables;
using NIST.CVP.Libraries.Shared.Results;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Services
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
		List<string> GetEmailAddresses(long personID);
	}
}