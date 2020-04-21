using NIST.CVP.Libraries.Shared.Enumerables;
using NIST.CVP.Libraries.Internal.ACVPCore.Models;
using NIST.CVP.Libraries.Internal.ACVPCore.Models.Parameters;
using NIST.CVP.Libraries.Shared.Results;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Providers
{
	public interface IPersonProvider
	{
		Person Get(long personID);
		PagedEnumerable<PersonLite> Get(PersonListParameters param);
		Result Delete(long personID);
		Result DeleteAllEmails(long personID);
		Result DeleteAllPhoneNumbers(long personID);
		InsertResult Insert(string name, long organizationID);
		Result InsertEmailAddress(long personID, string emailAddress, int orderIndex);
		Result InsertPhoneNumber(long personID, string type, string number, int orderIndex);
		bool PersonIsUsed(long personID);
		bool PersonExists(long personID);
		Result Update(long personID, string name, long? organizationID, bool nameUpdated, bool organizationIDUpdated);
	}
}