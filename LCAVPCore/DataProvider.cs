using System.Collections.Generic;
using System.Linq;
using CVP.DatabaseInterface;
using Mighty;

namespace LCAVPCore
{
	public class DataProvider : IDataProvider
	{
		private readonly string _acvpConnectionString;

		public DataProvider(IConnectionStringFactory connectionStringFactory)
		{
			_acvpConnectionString = connectionStringFactory.GetMightyConnectionString("ACVP");
		}

		public int GetVendorID(ChangeFile changeFile)
		{
			try
			{
				var db = new MightyOrm(_acvpConnectionString);

				var (Algorithm, CertNumber) = GetAnAlgoValidation(changeFile.AffectedValidations);

				var data = db.Query("EXEC [lcavp].[VendorIDGet] @0, @1", Algorithm, CertNumber).FirstOrDefault();

				return (int)data.VendorID;
			}
			catch
			{
				return -1;
			}
		}

		public int GetModuleID(ChangeFile changeFile)
		{
			try
			{
				var db = new MightyOrm(_acvpConnectionString);

				var (Algorithm, CertNumber) = GetAnAlgoValidation(changeFile.AffectedValidations);

				var data = db.Query("EXEC [lcavp].[ModuleIDGet] @0, @1", Algorithm, CertNumber).FirstOrDefault();

				return (int)data.ModuleID;
			}
			catch
			{
				return -1;
			}
		}

		public int GetPersonIDFromContact(ChangeFile changeFile, int orderIndex)
		{
			try
			{
				var db = new MightyOrm(_acvpConnectionString);

				var (Algorithm, CertNumber) = GetAnAlgoValidation(changeFile.AffectedValidations);

				var data = db.Query("EXEC [lcavp].[ContactPersonIDGet] @0, @1, @2", Algorithm, CertNumber, orderIndex).FirstOrDefault();

				return (int)data.PersonID;
			}
			catch
			{
				return -1;
			}
		}

		public int GetOEID(ChangeFile changeFile)
		{
			string processor = changeFile.UpdatedFields["_processor"].CurrentValue;
			string operatingSystem = changeFile.UpdatedFields["_operatingsystem"].CurrentValue;

			string oeName;

			if (string.IsNullOrEmpty(processor))
			{
				if (string.IsNullOrEmpty(operatingSystem))
				{
					//Extra special case where both processor and OS are blank/NA --> use the implementation name
					oeName = changeFile.ImplementationName;
				}
				else
				{
					oeName = operatingSystem;
				}
			}
			else if (string.IsNullOrEmpty(operatingSystem))
			{
				oeName = processor;
			}
			else
			{
				oeName = $"{operatingSystem} on {processor}";
			}

			try
			{
				var db = new MightyOrm(_acvpConnectionString);

				var (Algorithm, CertNumber) = GetAnAlgoValidation(changeFile.AffectedValidations);

				var data = db.Query("EXEC [lcavp].[OEIDGet] @0, @1, @2", Algorithm, CertNumber, oeName).FirstOrDefault();

				return (int)data.OEID;
			}
			catch
			{
				return -1;
			}
		}

		public List<int> GetOEIDs(ChangeFile changeFile)
		{
			string processor = changeFile.UpdatedFields["_processor"].CurrentValue;
			string operatingSystem = changeFile.UpdatedFields["_operatingsystem"].CurrentValue;

			string oeName;

			if (string.IsNullOrEmpty(processor))
			{
				if (string.IsNullOrEmpty(operatingSystem))
				{
					//Extra special case where both processor and OS are blank/NA --> use the implementation name
					oeName = changeFile.ImplementationName;
				}
				else
				{
					oeName = operatingSystem;
				}
			}
			else if (string.IsNullOrEmpty(operatingSystem))
			{
				oeName = processor;
			}
			else
			{
				oeName = $"{operatingSystem} on {processor}";
			}

			try
			{
				List<int> oes = new List<int>();

				var db = new MightyOrm(_acvpConnectionString);

				//Need to get the matching OEs from every referenced validation, because on updates to old validations Harold's code creates a new OE for each validation
				foreach ((string Algorithm, int CertNumber) in changeFile.AffectedValidations)
				{
					//There may be more than 1 OE with this name on this validation, due to a number of things that ideally wouldn't happen but do happen
					var data = db.Query("EXEC [lcavp].[OEIDGet] @0, @1, @2", Algorithm, CertNumber, oeName);
					foreach (var row in data)
					{
						oes.Add((int)row.OEID);
					}

				}

				//Since the same OE ID may be used multiple times (and hopefully they're all the same), return the distinct list
				return oes.Distinct().ToList();
			}
			catch
			{
				return new List<int>();
			}
		}


		private (string Algorithm, int CertNumber) GetAnAlgoValidation(List<(string Algorithm, int CertNumber)> affectedValidations)
		{
			return affectedValidations.FirstOrDefault();
		}

		public int GetValidationRecordID(string family, int certNumber)
		{
			try
			{
				var db = new MightyOrm(_acvpConnectionString);

				var data = db.Query("EXEC [lcavp].[ValidationRecordIDGet] @0, @1", family, certNumber).FirstOrDefault();

				return (int)data.ValidationRecordID;
			}
			catch
			{
				return -1;
			}
		}

		public int GetCValidationRecordIDForProduct(int moduleID)
		{
			try
			{
				var db = new MightyOrm(_acvpConnectionString);

				var data = db.Query("EXEC [lcavp].[CValidationRecordIDForModuleGet] @0", moduleID).FirstOrDefault();

				return (int)data.ValidationRecordID;
			}
			catch
			{
				return -1;
			}
		}

		public int GetValidationRecordIDForModuleAndAlgo(int moduleID, string family)
		{
			//This needs to handle it actually being C, family will never be C
			//First try to get it like is is non-C
			try
			{
				var db = new MightyOrm(_acvpConnectionString);

				var data = db.Query("EXEC [lcavp].[ValidationRecordIDForFamilyAndModuleGet] @0, @1", moduleID, family).FirstOrDefault();

				if (data != null)
				{
					return (int)data.ValidationRecordID;
				}
				else
				{
					return GetCValidationRecordIDForProduct(moduleID);
				}
			}
			catch
			{
				return -1;
			}



		}

		public int GetAddressID(int vendorID, ChangeFile changeFile)
		{
			try
			{
				var db = new MightyOrm(_acvpConnectionString);

				var (Algorithm, CertNumber) = GetAnAlgoValidation(changeFile.AffectedValidations);

				//Get the original address fields - only some are populated
				string address1 = null
					, address2 = null
					, address3 = null
					, locality = null
					, region = null
					, postalCode = null
					, country = null;

				if (changeFile.UpdatedFields.ContainsKey("Address1")) address1 = CleanValue(changeFile.UpdatedFields.GetValue("Address1").CurrentValue);
				if (changeFile.UpdatedFields.ContainsKey("Address2")) address2 = CleanValue(changeFile.UpdatedFields.GetValue("Address2").CurrentValue);
				if (changeFile.UpdatedFields.ContainsKey("Address3")) address3 = CleanValue(changeFile.UpdatedFields.GetValue("Address3").CurrentValue);
				if (changeFile.UpdatedFields.ContainsKey("City")) locality = changeFile.UpdatedFields.GetValue("City").CurrentValue;
				if (changeFile.UpdatedFields.ContainsKey("State")) region = changeFile.UpdatedFields.GetValue("State").CurrentValue;
				if (changeFile.UpdatedFields.ContainsKey("Zip")) postalCode = changeFile.UpdatedFields.GetValue("Zip").CurrentValue;
				if (changeFile.UpdatedFields.ContainsKey("Country")) country = changeFile.UpdatedFields.GetValue("Country").CurrentValue;

				var data = db.Query("EXEC [lcavp].[VendorAddressIDGet] @0, @1, @2, @3, @4, @5, @6, @7", vendorID, address1, address2, address3, locality, region, postalCode, country).FirstOrDefault();

				return (int)data.AddressID;
			}
			catch
			{
				return -1;
			}
		}

		private string CleanValue(string value)
		{
			//Trim, scrub default garbage, and make null if left with an empty string
			string cleanedValue = value?.Trim().Replace("n/a", "");
			return string.IsNullOrEmpty(cleanedValue) ? null : cleanedValue;
		}

		public List<string> GetProductNameForValidations(List<(string Algorithm, int CertNumber)> affectedValidations)
		{
			List<string> names = new List<string>();
			var db = new MightyOrm(_acvpConnectionString);

			//Get the product name for each validation - hopefully they're all the same, and there's just 1 product
			foreach ((string Algorithm, int CertNumber) in affectedValidations)
			{
				try
				{
					var data = db.Query("EXEC [lcavp].[ProductNameForValidationGet] @0, @1", Algorithm, CertNumber).FirstOrDefault();

					names.Add((string)data.ProductName);
				}
				catch
				{
					return new List<string>();
				}
			}

			return names.Distinct().ToList();
		}

		public List<(int OrderIndex, string PhoneNumber, string Type)> GetPhoneNumbersForPerson(int personID)
		{
			try
			{
				List<(int OrderIndex, string PhoneNumber, string Type)> phoneNumbers = new List<(int OrderIndex, string PhoneNumber, string Type)>();

				var db = new MightyOrm(_acvpConnectionString);

				//There may be more than 1 OE with this name on this validation, due to a number of things that ideally wouldn't happen but do happen
				var data = db.Query("EXEC [lcavp].[PersonPhoneNumbersGet] @0", personID);
				foreach (var row in data)
				{
					phoneNumbers.Add(((int)row.OrderIndex, (string)row.PhoneNumber, (string)row.Type));
				}

				return phoneNumbers;
			}
			catch
			{
				return new List<(int OrderIndex, string PhoneNumber, string Type)>();
			}


		}
	}
}
