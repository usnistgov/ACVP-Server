using ACVPCore.Models;
using ACVPCore.Results;

namespace ACVPCore.Providers
{
	public interface IOrganizationProvider
	{
		InsertResult Insert(string name, string website, string voiceNumber, string faxNumber, long? parentOrganizationID);
		Result InsertEmailAddress(long organizationID, string emailAddress, int orderIndex);

		Result Update(long organizationID, string name, string website, string voiceNumber, string faxNumber, long? parentOrganizationID, bool nameUpdated, bool websiteUpdated, bool voiceNumberUpdated, bool faxNumberUpdated, bool parentOrganizationIDUpdated);
		Organization Get(long organizationID);
		Result Delete(long organizationID);
		Result DeleteAllEmails(long organizationID);

		bool OrganizationIsUsed(long organizationID);
	}
}