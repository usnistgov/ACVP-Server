using System.Collections.Generic;

namespace LCAVPCore
{
	public interface IDataProvider
	{
		int GetPersonIDFromContact(ChangeFile changeFile, int orderIndex);
		int GetModuleID(ChangeFile changeFile);
		int GetVendorID(ChangeFile changeFile);
		int GetOEID(ChangeFile changeFile);
		List<int> GetOEIDs(ChangeFile changeFile);
		int GetValidationRecordID(string family, int certNumber);
		int GetAddressID(int vendorID, ChangeFile changeFile);
		int GetCValidationRecordIDForProduct(int moduleID);
		List<string> GetProductNameForValidations(List<(string Algorithm, int CertNumber)> affectedValidations);
		int GetValidationRecordIDForModuleAndAlgo(int moduleID, string family);
		List<(int OrderIndex, string PhoneNumber, string Type)> GetPhoneNumbersForPerson(int personID);
		long GetValidationIDForSubmissionID(string submissionID);
		long GetValidationNumberForValidationID(long validationID);
	}
}