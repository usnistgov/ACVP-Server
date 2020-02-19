using ACVPCore.Models;
using ACVPCore.Results;

namespace ACVPCore.Providers
{
	public interface IImplementationProvider
	{
		Result Delete(long implementationID);
		Implementation Get(long implementationID);
		Result DeleteAllContacts(long implementationID);
		bool ImplementationIsUsed(long implementationID);
		bool ImplementationExists(long implementationID);
		InsertResult Insert(string name, string description, ImplementationType type, string version, string website, long organizationID, long addressID, bool isITAR);
		Result InsertContact(long implementationID, long personID, int orderIndex);
		Result Update(long implementationID, string name, string description, ImplementationType type, string version, string website, long? organizationID, long? addressID, bool nameUpdated, bool descriptionUpdated, bool typeUpdated, bool versionUpdated, bool websiteUpdated, bool organizationIDUpdated, bool addressIDUpdated);
	}
}