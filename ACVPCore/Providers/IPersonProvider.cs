using ACVPCore.Models;
using ACVPCore.Results;
using System.Collections.Generic;

namespace ACVPCore.Providers
{
	public interface IPersonProvider
	{
		Person Get(long personID);
		List<PersonLite> Get(long pageSize, long pageNumber);
		Result Delete(long personID);
		Result DeleteAllEmails(long personID);
		Result DeleteAllPhoneNumbers(long personID);
		InsertResult Insert(string name, long organizationID);
		Result InsertEmailAddress(long personID, string emailAddress, int orderIndex);
		Result InsertPhoneNumber(long personID, string type, string number, int orderIndex);
		bool PersonIsUsed(long personID);
		Result Update(long personID, string name, long? organizationID, bool nameUpdated, bool organizationIDUpdated);
	}
}