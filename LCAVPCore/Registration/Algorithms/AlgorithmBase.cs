using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace LCAVPCore.Registration.Algorithms
{
	public abstract class AlgorithmBase : IAlgorithm
	{
		//[JsonProperty(PropertyName = "algorithm", Order = -4)]
		[JsonIgnore]
		public string Algorithm { get; private set; }

		[JsonIgnore]
		public int AlgorithmID { get => GetAlgorithmID(); }

		[JsonIgnore]
		public string Family { get => GetFamily(); }

		//[JsonProperty(PropertyName = "mode", Order = -3, NullValueHandling = NullValueHandling.Ignore)]
		[JsonIgnore]
		public string Mode { get; private set; }

		//[JsonProperty(PropertyName = "revision", Order = -3, NullValueHandling = NullValueHandling.Ignore)]
		[JsonIgnore]
		public string Revision { get; private set; } = "1.0";


		//Prereqs are a little weird... Only want to serialize if there are prereqs (since many algos don't have any), so need to null it out. But also may have null prereqs in collection if optional prereqs are not used
		//So the code populates one property, PreReqs, without worrying about this
		//Then PreReqsToOutput massages PreReqs to get the right result
		//The massaging can't bedone within PreReqs's getter, as an Add calls the getter, which returns a null when the list is empty (like the first time around) - and it blows up
		//Calling PreReqsToOutput's getter has a side effect of modifying PreReqs, which is funky, but not a problem
		//Perhaps it should also take the unique values, just to be safe...
		[JsonIgnore]
		public List<Prerequisite> CleanPreReqs
		{
			get
			{
				PreReqs.RemoveAll(x => x == null);     //Remove any nulls that might have been added by "n/a" values
				return PreReqs.Count > 0 ? PreReqs.Distinct(new PrerequisiteComparer()).ToList() : null;
			}
		}

		[JsonIgnore]
		public List<Prerequisite> PreReqs { get; set; } = new List<Prerequisite>();

		[JsonIgnore]
		public List<string> Errors { get; set; } = new List<string>();

		public AlgorithmBase(string algorithm, string mode = null)
		{
			Algorithm = algorithm;
			Mode = mode;
		}

		public AlgorithmBase(string algorithm, string mode, string revision)
		{
			Algorithm = algorithm;
			Mode = mode;
			Revision = revision;
		}

		public Prerequisite BuildPrereq(string algorithm, string value)
		{
			Prerequisite prereq = new Prerequisite();
			prereq.Algorithm = algorithm;

			switch (value)
			{
				case "In this same implementation":
					//prereq.ValidationID = "same";
					break;
				case "n/a":         //Don't want to create a prereq...
				case null:
					prereq = null;
					return prereq;
				default:
					//Not a special string, so either a cert number or a reference to a submission. First, see if it is a number
					int certNumber;
					bool isNumber = int.TryParse(value, out certNumber);
					string searchAlgorithm = algorithm;	//Default need to look up what was passed in, but if an A/C need to search for A/C

					if (!isNumber)
					{
						//Might be A# or C#, so handle that
						Regex validationPattern = new Regex(@"^(?<prefix>[AC])\s*(?<number>\d+)$");

						Match match = validationPattern.Match(value);
						if (match.Success)
						{
							isNumber = true;
							searchAlgorithm = match.Groups["prefix"].Value;
							certNumber = int.Parse(match.Groups["number"].Value);
						}
					}

					//Now if we've managed to get to a number find the record
					if (isNumber)
					{
						//Look up the validation record ID
						int validationRecordID = new DataProvider().GetValidationRecordID(searchAlgorithm, certNumber);
						if (validationRecordID > 0)
						{
							prereq.ValidationRecordID = validationRecordID;
						}
						else
						{
							//Couldn't find a matching validation, so just ignore the prereq
							prereq = null;
							return prereq;
						}
					}
					else
					{
						//Assume it is a reference to a submission ID
						prereq.SubmissionID = value;
					}
					break;
			}

			return prereq;
		}

		private int GetAlgorithmID()
		{
			switch ($"{Algorithm}|{Mode}|{Revision}")
			{
				case "ACVP-AES-CBC||1.0": return 2;
				case "ACVP-AES-CCM||1.0": return 7;
				case "ACVP-AES-CFB1||1.0": return 3;
				case "ACVP-AES-CFB128||1.0": return 5;
				case "ACVP-AES-CFB8||1.0": return 4;
				case "ACVP-AES-CTR||1.0": return 53;
				case "ACVP-AES-ECB||1.0": return 1;
				case "ACVP-AES-GCM||1.0": return 0;
				case "ACVP-AES-KW||1.0": return 20;
				case "ACVP-AES-KWP||1.0": return 49;
				case "ACVP-AES-OFB||1.0": return 6;
				case "ACVP-AES-XPN||1.0": return 47;
				case "ACVP-AES-XTS||1.0": return 48;
				case "CMAC-AES||1.0": return 42;
				case "CMAC-TDES||1.0": return 43;
				case "ctrDRBG||1.0": return 19;
				case "DSA|keyGen|1.0": return 57;
				case "DSA|keyGen|186-2": return 108;
				case "DSA|pqgGen|1.0": return 65;
				case "DSA|pqgGen|186-2": return 111;
				case "DSA|pqgVer|1.0": return 66;
				case "DSA|pqgVer|186-2": return 112;
				case "DSA|primality|186-2": return 113;
				case "DSA|sigGen|1.0": return 64;
				case "DSA|sigGen|186-2": return 110;
				case "DSA|sigVer|1.0": return 63;
				case "DSA|sigVer|186-2": return 109;
				case "dualEC_DRBG||1.0": return 107;
				case "ECDSA|keyGen|1.0": return 67;
				case "ECDSA|keyGen|186-2": return 114;
				case "ECDSA|keyVer|1.0": return 68;
				case "ECDSA|keyVer|186-2": return 115;
				case "ECDSA|sigGen|1.0": return 69;
				case "ECDSA|sigGen|186-2": return 116;
				case "ECDSA|sigVer|1.0": return 70;
				case "ECDSA|sigVer|186-2": return 117;
				case "hashDRBG||1.0": return 61;
				case "hmacDRBG||1.0": return 62;
				case "HMAC-SHA-1||1.0": return 23;
				case "HMAC-SHA2-224||1.0": return 24;
				case "HMAC-SHA2-256||1.0": return 25;
				case "HMAC-SHA2-384||1.0": return 26;
				case "HMAC-SHA2-512||1.0": return 27;
				case "HMAC-SHA2-512/224||1.0": return 28;
				case "HMAC-SHA2-512/256||1.0": return 29;
				case "HMAC-SHA3-224||1.0": return 30;
				case "HMAC-SHA3-256||1.0": return 31;
				case "HMAC-SHA3-384||1.0": return 32;
				case "HMAC-SHA3-512||1.0": return 33;
				case "KAS-ECC||1.0": return 72;
				case "KAS-ECC|CDH-Component|1.0": return 84;
				case "KAS-ECC|Component|1.0": return 86;
				case "KAS-FFC||1.0": return 71;
				case "KAS-FFC|Component|1.0": return 85;
				case "KDF||1.0": return 80;
				case "kdf-components|ansix9.63|1.0": return 79;
				case "kdf-components|ikev1|1.0": return 73;
				case "kdf-components|ikev2|1.0": return 74;
				case "kdf-components|snmp|1.0": return 76;
				case "kdf-components|srtp|1.0": return 77;
				case "kdf-components|ssh|1.0": return 75;
				case "kdf-components|tls|1.0": return 78;
				case "RSA|decryptionPrimitive|1.0": return 81;
				case "RSA|keyGen|1.0": return 58;
				case "RSA|keyGen|186-2": return 118;
				case "RSA|legacySigVer|1.0": return 83;
				case "RSA|sigGen|1.0": return 59;
				case "RSA|sigGen|186-2": return 119;
				case "RSA|signaturePrimitive|1.0": return 82;
				case "RSA|sigVer|1.0": return 60;
				case "SHA-1||1.0": return 12;
				case "SHA2-224||1.0": return 13;
				case "SHA2-256||1.0": return 14;
				case "SHA2-384||1.0": return 15;
				case "SHA2-512||1.0": return 16;
				case "SHA2-512/224||1.0": return 17;
				case "SHA2-512/256||1.0": return 18;
				case "SHA3-224||1.0": return 34;
				case "SHA3-256||1.0": return 35;
				case "SHA3-384||1.0": return 36;
				case "SHA3-512||1.0": return 37;
				case "SHAKE-128||1.0": return 38;
				case "SHAKE-256||1.0": return 39;
				case "ACVP-TDES-CBC||1.0": return 9;
				case "ACVP-TDES-CBCI||1.0": return 55;
				case "ACVP-TDES-CFB1||1.0": return 44;
				case "ACVP-TDES-CFB64||1.0": return 46;
				case "ACVP-TDES-CFB8||1.0": return 45;
				case "ACVP-TDES-CFBP1||1.0": return 50;
				case "ACVP-TDES-CFBP64||1.0": return 52;
				case "ACVP-TDES-CFBP8||1.0": return 51;
				case "ACVP-TDES-CTR||1.0": return 54;
				case "ACVP-TDES-ECB||1.0": return 10;
				case "ACVP-TDES-KW||1.0": return 21;
				case "ACVP-TDES-OFB||1.0": return 22;
				case "ACVP-TDES-OFBI||1.0": return 56;
				default: return -1;
			}
		}

		private string GetFamily()
		{
			switch (AlgorithmID)
			{
				case 0: return "AES";
				case 1: return "AES";
				case 2: return "AES";
				case 3: return "AES";
				case 4: return "AES";
				case 5: return "AES";
				case 6: return "AES";
				case 7: return "AES";
				case 20: return "AES";
				case 42: return "AES";
				case 47: return "AES";
				case 48: return "AES";
				case 49: return "AES";
				case 53: return "AES";
				case 73: return "Component";
				case 74: return "Component";
				case 75: return "Component";
				case 76: return "Component";
				case 77: return "Component";
				case 78: return "Component";
				case 79: return "Component";
				case 81: return "Component";
				case 82: return "Component";
				case 84: return "Component";
				case 85: return "Component";
				case 86: return "Component";
				case 19: return "DRBG";
				case 61: return "DRBG";
				case 62: return "DRBG";
				case 107: return "DRBG";
				case 57: return "DSA";
				case 63: return "DSA";
				case 64: return "DSA";
				case 65: return "DSA";
				case 66: return "DSA";
				case 108: return "DSA";
				case 109: return "DSA";
				case 110: return "DSA";
				case 111: return "DSA";
				case 112: return "DSA";
				case 113: return "DSA";
				case 67: return "ECDSA";
				case 68: return "ECDSA";
				case 69: return "ECDSA";
				case 70: return "ECDSA";
				case 114: return "ECDSA";
				case 115: return "ECDSA";
				case 116: return "ECDSA";
				case 117: return "ECDSA";
				case 23: return "HMAC";
				case 24: return "HMAC";
				case 25: return "HMAC";
				case 26: return "HMAC";
				case 27: return "HMAC";
				case 28: return "HMAC";
				case 29: return "HMAC";
				case 30: return "HMAC";
				case 31: return "HMAC";
				case 32: return "HMAC";
				case 33: return "HMAC";
				case 71: return "KAS";
				case 72: return "KAS";
				case 80: return "KDF";
				case 58: return "RSA";
				case 59: return "RSA";
				case 60: return "RSA";
				case 83: return "RSA";
				case 118: return "RSA";
				case 119: return "RSA";
				case 34: return "SHA-3";
				case 35: return "SHA-3";
				case 36: return "SHA-3";
				case 37: return "SHA-3";
				case 38: return "SHA-3";
				case 39: return "SHA-3";
				case 12: return "SHS";
				case 13: return "SHS";
				case 14: return "SHS";
				case 15: return "SHS";
				case 16: return "SHS";
				case 17: return "SHS";
				case 18: return "SHS";
				case 9: return "TDES";
				case 10: return "TDES";
				case 21: return "TDES";
				case 22: return "TDES";
				case 43: return "TDES";
				case 44: return "TDES";
				case 45: return "TDES";
				case 46: return "TDES";
				case 50: return "TDES";
				case 51: return "TDES";
				case 52: return "TDES";
				case 54: return "TDES";
				case 55: return "TDES";
				case 56: return "TDES";
				default: return null;
			}
		}

	}
}
