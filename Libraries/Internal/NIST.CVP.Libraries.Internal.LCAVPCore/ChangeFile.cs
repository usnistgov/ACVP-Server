using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NIST.CVP.Libraries.Internal.LCAVPCore
{
	public class ChangeFile
	{
		private string FilePath { get; set; }

		public bool Valid { get { return Errors.Count == 0; } }

		public List<string> Errors { get; set; } = new List<string>();

		public Dictionary<string, (string CurrentValue, string UpdatedValue)> UpdatedFields = new Dictionary<string, (string CurrentValue, string UpdatedValue)>();

		private static List<(string FieldName, DataTypes DataType)> UpdateFieldNames = new List<(string FieldName, DataTypes DataChunk)> {  ("VendorName", DataTypes.Vendor),
																																			("WebSite", DataTypes.Vendor),
																																			("ImplName", DataTypes.Module),
																																			("ImplDesc", DataTypes.Module),
																																			("Software", DataTypes.TypeAndVersion),
																																			("Firmware", DataTypes.TypeAndVersion),
																																			("Hardware", DataTypes.TypeAndVersion),
																																			("SoftVersion", DataTypes.TypeAndVersion),
																																			("FirmVersion", DataTypes.TypeAndVersion),
																																			("PartNo", DataTypes.TypeAndVersion),
																																			("Address1", DataTypes.Address),
																																			("Address2", DataTypes.Address),
																																			("Address3", DataTypes.Address),
																																			("City", DataTypes.Address),
																																			("State", DataTypes.Address),
																																			("Zip", DataTypes.Address),
																																			("Country", DataTypes.Address),
																																			("ContactName", DataTypes.Contact1),
																																			("Email", DataTypes.Contact1),
																																			("PhoneNo", DataTypes.Contact1),
																																			("FAXNo", DataTypes.Contact1),
																																			("Contact2Name", DataTypes.Contact2),
																																			("Email2", DataTypes.Contact2),
																																			("PhoneNo2", DataTypes.Contact2),
																																			("FAXNo2", DataTypes.Contact2),
																																			("_processor", DataTypes.OE),
																																			("_operatingsystem", DataTypes.OE)};

		private enum DataTypes
		{
			Vendor,
			Module,
			Address,
			Contact1,
			Contact2,
			OE,
			TypeAndVersion
		}


		public string VendorName { get; set; }
		public string ImplementationName { get; set; }

		public List<(string Algorithm, int CertNumber)> AffectedValidations { get; set; } = new List<(string Algorithm, int CertNumber)>();

		public bool IncludesChange
		{
			get
			{
				//Since a "change" file may actually be an from an update that only added algos/OEs, and thus didn't actually do a change, there may be no changes in the change file (just additions, but those are handled elsewhere
				return IncludesVendorChange || IncludesModuleChange || IncludesAddressChange || IncludesContactChange || IncludesOEChange || IncludesTypeVersionChange;
			}
		}

		public bool IncludesVendorChange
		{
			get
			{
				//string[] keys = { "UpdatedVendorName", "UpdatedWebSite" };
				//return keys.Any(k => UpdatedFields.ContainsKey(k));
				return IncludesChangeOfType(DataTypes.Vendor);
			}
		}

		public bool IncludesModuleChange
		{
			get
			{
				//string[] keys = { "UpdatedImplName", "UpdatedImplDesc", "UpdatedSoftware", "UpdatedFirmware", "UpdatedHardware", "UpdatedSoftVersion", "UpdatedFirmVersion", "UpdatedPartNo" };
				//return keys.Any(k => UpdatedFields.ContainsKey(k));
				return IncludesChangeOfType(DataTypes.Module) || IncludesTypeVersionChange;
			}
		}

		public bool IncludesAddressChange
		{
			get
			{
				//string[] keys = { "UpdatedAddress1", "UpdatedAddress2", "UpdatedAddress3", "UpdatedCity", "UpdatedState", "UpdatedZip", "UpdatedCountry" };
				//return keys.Any(k => UpdatedFields.ContainsKey(k));
				return IncludesChangeOfType(DataTypes.Address);
			}
		}

		public bool IncludesContactChange
		{
			get
			{
				return IncludesContact1Change || IncludesContact2Change;
			}
		}

		public bool IncludesContact1Change
		{
			get
			{
				//string[] keys = { "UpdatedContactName", "UpdatedEmail", "UpdatedPhoneNo", "UpdatedFAXNo" };
				//return keys.Any(k => UpdatedFields.ContainsKey(k));
				return IncludesChangeOfType(DataTypes.Contact1);
			}
		}

		public bool IncludesContact1Add
		{
			get
			{
				return IncludesContact1Change
				&& (!UpdatedFields.ContainsKey("ContactName") || (UpdatedFields.ContainsKey("ContactName") && string.IsNullOrEmpty(UpdatedFields.GetValue("ContactName").CurrentValue)))
				&& (!UpdatedFields.ContainsKey("Email") || (UpdatedFields.ContainsKey("Email") && string.IsNullOrEmpty(UpdatedFields.GetValue("Email").CurrentValue)))
				&& (!UpdatedFields.ContainsKey("PhoneNo") || (UpdatedFields.ContainsKey("PhoneNo") && string.IsNullOrEmpty(UpdatedFields.GetValue("PhoneNo").CurrentValue)))
				&& (!UpdatedFields.ContainsKey("FAXNo") || (UpdatedFields.ContainsKey("FAXNo") && string.IsNullOrEmpty(UpdatedFields.GetValue("FAXNo").CurrentValue)));
			}
		}

		public bool IncludesContact2Change
		{
			get
			{
				//string[] keys = { "UpdatedContact2Name", "UpdatedEmail2", "UpdatedPhoneNo2", "UpdatedFAXNo2" };
				//return keys.Any(k => UpdatedFields.ContainsKey(k));
				return IncludesChangeOfType(DataTypes.Contact2);
			}
		}

		public bool IncludesContact2Add
		{
			get
			{
				return IncludesContact2Change
				&& (!UpdatedFields.ContainsKey("Contact2Name") || (UpdatedFields.ContainsKey("Contact2Name") && string.IsNullOrEmpty(UpdatedFields.GetValue("Contact2Name").CurrentValue)))
				&& (!UpdatedFields.ContainsKey("Email2") || (UpdatedFields.ContainsKey("Email2") && string.IsNullOrEmpty(UpdatedFields.GetValue("Email2").CurrentValue)))
				&& (!UpdatedFields.ContainsKey("PhoneNo2") || (UpdatedFields.ContainsKey("PhoneNo2") && string.IsNullOrEmpty(UpdatedFields.GetValue("PhoneNo2").CurrentValue)))
				&& (!UpdatedFields.ContainsKey("FAXNo2") || (UpdatedFields.ContainsKey("FAXNo2") && string.IsNullOrEmpty(UpdatedFields.GetValue("FAXNo2").CurrentValue)));
			}
		}

		public bool IncludesOEChange
		{
			get
			{
				//string[] keys = { "Updated_processor", "Updated_operatingsystem" };
				//return keys.Any(k => UpdatedFields.ContainsKey(k));
				return IncludesChangeOfType(DataTypes.OE);
			}
		}

		public bool IncludesTypeVersionChange
		{
			get
			{
				//string[] keys = { "UpdatedSoftware", "UpdatedFirmware", "UpdatedHardware", "UpdatedSoftVersion", "UpdatedFirmVersion", "UpdatedPartNo" };
				//return keys.Any(k => UpdatedFields.ContainsKey(k));
				return IncludesChangeOfType(DataTypes.TypeAndVersion);
			}
		}

		private bool IncludesChangeOfType(DataTypes type)
		{
			return UpdateFieldNames.Where(f => f.DataType == type).Any(f => UpdatedFields.ContainsKey(f.FieldName));
		}

		public ChangeFile(string filePath)
		{
			FilePath = filePath;
			Parse();
		}

		private void Parse()
		{
			//Pull all the lines of text into a list of strings. Use a try/catch in order to catch any weird things that happen trying to read the file - like it somehow doesn't exist, or is unreadable
			List<string> lines;

			try
			{
				lines = File.ReadAllLines(FilePath, Encoding.GetEncoding(1252)).ToList();
			}
			catch
			{
				Errors.Add($"Unable to read file {FilePath}");
				return;
			}

			//Fail if no lines
			if (lines == null)
			{
				Errors.Add("Change file is empty");
				return;
			}

			//Extract the vendor_name and implementation_name values, which are needed to identify the vendor and module to apply updates to
			try
			{
				VendorName = lines.First(l => l.StartsWith("vendor_name")).Split("=".ToCharArray())[1].Trim();
				ImplementationName = lines.First(l => l.StartsWith("implementation_name")).Split("=".ToCharArray())[1].Trim();
			}
			catch
			{
				Errors.Add("Unable to identify vendor or implementation to change");
				return;
			}



			//Build a collection of string prefixes that we care about - don't want to get all of the Updated lines, as some we will not allow, and some are not actually updated (are just the identifiers)
			//string[] prefixes = { "UpdatedVendorName", "UpdatedWebSite", "UpdatedAddress1", "UpdatedAddress2", "UpdatedAddress3", "UpdatedCity", "UpdatedState", "UpdatedZip", "UpdatedCountry", "UpdatedContactName", "UpdatedEmail", "UpdatedPhoneNo", "UpdatedFAXNo", "UpdatedContact2Name", "UpdatedEmail2", "UpdatedPhoneNo2", "UpdatedFAXNo2", "UpdatedCAVSVer", "UpdatedImplName", "UpdatedImplDesc", "Updated_processor", "Updated_operatingsystem" };

			//Extract all the updated fields
			//foreach (var line in lines.Where(l => prefixes.Any(l.StartsWith)))      //This LINQ statement gets all the lines that start with any of the prefixes
			//var prefixesWeCareAbout = from x in linePrefixes from y in UpdateFieldNames select x + y.FieldName;

			var updateLines = lines.Where(l => UpdateFieldNames.Select(f => "Updated" + f.FieldName).Any(l.StartsWith));
			var currentLines = lines.Where(l => UpdateFieldNames.Select(f => "Current" + f.FieldName).Any(l.StartsWith));

			foreach (string updateLine in updateLines)
			{
				string[] parts = updateLine.Split("=".ToCharArray());
				string updatedValue = null;
				string currentValue = null;

				if (parts.Length == 2)
				{
					updatedValue = parts[1];
				}
				else if (parts.Length > 2)      //Must be a = in the value portion of the line, so recombine all the parts after the first into one value
				{
					updatedValue = string.Join("=", new ArraySegment<string>(parts, 1, parts.Length - 1));
				}
				else
				{
					Errors.Add($"Could not parse updated line: {updateLine}");
					return;
				}

				//Get the corresponding current line
				string fieldName = parts[0].Trim().Replace("Updated", "");		//Added this trim because otherwise this results in searching for "UpdatedFoo " if the update line has a space before the = (which is normal), which was problematic if the Current line didn't have the space. Edge case, but it happened
				string currentLine = currentLines.FirstOrDefault(l => l.StartsWith("Current" + fieldName));

				if (currentLine == null)
				{
					Errors.Add($"Could not find current value for {fieldName}");
					return;
				}
				else
				{
					parts = currentLine.Split("=".ToCharArray());

					if (parts.Length == 2)
					{
						currentValue = parts[1];
					}
					else if (parts.Length > 2)
					{
						currentValue = string.Join("=", new ArraySegment<string>(parts, 1, parts.Length - 1));
					}
					else
					{
						Errors.Add($"Could not parse current line: {currentLine}");
						return;
					}
				}

				//Add this property with current and updated values to update collection
				UpdatedFields.Add(fieldName.Trim(), (CleanValue(currentValue), CleanValue(updatedValue)));
			}


			//Since OS and processor are a pair that must go together, even if only 1 part is changing, verify that if they have 1 they have both
			if (UpdatedFields.Any(f => f.Key == "_operatingsystem") && !UpdatedFields.Any(f => f.Key == "_processor"))
			{
				Errors.Add("Processor is missing from [Change or Update]_Req_File.txt - current and updated values of both Operating System and Processor must be present if either is being changed");
				return;
			}

			if (UpdatedFields.Any(f => f.Key == "_processor") && !UpdatedFields.Any(f => f.Key == "_operatingsystem"))
			{
				Errors.Add("Operating System is missing from [Change or Update]_Req_File.txt - current and updated values of both Operating System and Processor must be present if either is being changed");
				return;
			}


			//Extract the validations to which this applies. Lines are of the format {algo}ValNo = {valNum}
			//Since this uses a capturing regex to get the algo and val num it can't be done a slick way (like in bulk, or through a lambda in the foreach). Just brute force it.
			Regex validationPattern = new Regex(@"^(?<algo>.+)ValNo = (?<valNum>[AC]?\d+)$");
			foreach (var line in lines)
			{
				Match match = validationPattern.Match(line);
				if (match.Success)
				{
					//Need to convert the algorithm listed to the family in our data model. Some match, some don't - Component is especially weird
					string family = ConvertChangeAlgoToFamily(match.Groups["algo"].Value);
					if (family != null)
					{
						//Now in case they use a new style validation number, need to manipulate the data so we return the right thing
						string validationNumber = match.Groups["valNum"].Value;
						string firstCharacter = validationNumber[0].ToString();
						if (firstCharacter == "A" || firstCharacter == "C")
						{
							//It is a new style validation
							family = firstCharacter;
							validationNumber = validationNumber.Substring(1);
						}
						AffectedValidations.Add((family, int.Parse(validationNumber)));
					}
				}
			}

			////Check that at least one validation to which these changes apply was found - otherwise this is useless
			//if (AffectedValidations.Count == 0)
			//{
			//	Errors.Add("No affected validations found in change file");
			//	return;
			//}
		}


		private string ConvertChangeAlgoToFamily(string algo)
		{
			switch (algo)
			{
				case "AES": return "AES";
				case "DRBG": return "DRBG";
				case "DSA": return "DSA";
				case "ECDSA": return "ECDSA";
				case "HMAC": return "HMAC";
				case "KAS": return "KAS";
				case "KDF108": return "KDF";
				case "RSA": return "RSA";
				case "SHA": return "SHS";
				case "SHA3": return "SHA-3";
				case "TDES": return "TDES";
				case "CVL1":
				case "CVL2":
				case "CVL3":
				case "CVL4":
				case "CVL5":
				case "CVL6":
					return "Component";
				default: return null;
			}
		}

		private string CleanValue(string value)
		{
			//Trim, scrub default garbage, and make null if left with an empty string
			string cleanedValue = value?.Trim().Replace("n/a", "").Replace("N/A", "");
			return string.IsNullOrEmpty(cleanedValue) ? null : cleanedValue;
		}
	}
}