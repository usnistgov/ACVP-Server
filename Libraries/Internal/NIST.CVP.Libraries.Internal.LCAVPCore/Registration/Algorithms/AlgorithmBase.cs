using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NIST.CVP.Libraries.Internal.Algorithms.Persisted;
using Newtonsoft.Json;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.Registration.Algorithms
{
	public abstract class AlgorithmBase // : IAlgorithm
	{
		private readonly IDataProvider _dataProvider;

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

		public AlgorithmBase(IDataProvider dataProvider, string algorithm, string mode = null)
		{
			_dataProvider = dataProvider;
			Algorithm = algorithm;
			Mode = mode;
		}

		public AlgorithmBase(IDataProvider dataProvider, string algorithm, string mode, string revision)
		{
			_dataProvider = dataProvider;
			Algorithm = algorithm;
			Mode = mode;
			Revision = revision;
		}

		public Prerequisite BuildPrereq(string algorithm, string value)
		{
			Prerequisite prereq = new Prerequisite
			{
				Algorithm = algorithm
			};

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
					string searchAlgorithm = algorithm; //Default need to look up what was passed in, but if an A/C need to search for A/C

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
						int validationRecordID = _dataProvider.GetValidationRecordID(searchAlgorithm, certNumber);
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

						//Need to look up the submission to get the validation ID, but can't do that here - way inappropriate to pass the validation ID here
					}
					break;
			}

			return prereq;
		}

		private int GetAlgorithmID() => $"{Algorithm}|{Mode}|{Revision}" switch
		{
			"ACVP-AES-CBC||1.0" => 2,
			"ACVP-AES-CCM||1.0" => 7,
			"ACVP-AES-CFB1||1.0" => 3,
			"ACVP-AES-CFB128||1.0" => 5,
			"ACVP-AES-CFB8||1.0" => 4,
			"ACVP-AES-CTR||1.0" => 53,
			"ACVP-AES-ECB||1.0" => 1,
			"ACVP-AES-GCM||1.0" => 0,
			"ACVP-AES-GMAC||1.0" => 141,
			"ACVP-AES-KW||1.0" => 20,
			"ACVP-AES-KWP||1.0" => 49,
			"ACVP-AES-OFB||1.0" => 6,
			"ACVP-AES-XPN||1.0" => 47,
			"ACVP-AES-XTS||1.0" => 48,
			"CMAC-AES||1.0" => 42,
			"CMAC-TDES||1.0" => 43,
			"ctrDRBG||1.0" => 19,
			"DSA|keyGen|1.0" => 57,
			"DSA|keyGen|186-2" => 108,
			"DSA|pqgGen|1.0" => 65,
			"DSA|pqgGen|186-2" => 111,
			"DSA|pqgVer|1.0" => 66,
			"DSA|pqgVer|186-2" => 112,
			"DSA|primality|186-2" => 113,
			"DSA|sigGen|1.0" => 64,
			"DSA|sigGen|186-2" => 110,
			"DSA|sigVer|1.0" => 63,
			"DSA|sigVer|186-2" => 109,
			"dualEC_DRBG||1.0" => 107,
			"ECDSA|keyGen|1.0" => 67,
			"ECDSA|keyGen|186-2" => 114,
			"ECDSA|keyVer|1.0" => 68,
			"ECDSA|keyVer|186-2" => 115,
			"ECDSA|sigGen|1.0" => 69,
			"ECDSA|sigGen|186-2" => 116,
			"ECDSA|sigVer|1.0" => 70,
			"ECDSA|sigVer|186-2" => 117,
			"hashDRBG||1.0" => 61,
			"hmacDRBG||1.0" => 62,
			"HMAC-SHA-1||1.0" => 23,
			"HMAC-SHA2-224||1.0" => 24,
			"HMAC-SHA2-256||1.0" => 25,
			"HMAC-SHA2-384||1.0" => 26,
			"HMAC-SHA2-512||1.0" => 27,
			"HMAC-SHA2-512/224||1.0" => 28,
			"HMAC-SHA2-512/256||1.0" => 29,
			"HMAC-SHA3-224||1.0" => 30,
			"HMAC-SHA3-256||1.0" => 31,
			"HMAC-SHA3-384||1.0" => 32,
			"HMAC-SHA3-512||1.0" => 33,
			"KAS-ECC||1.0" => 72,
			"KAS-ECC|CDH-Component|1.0" => 84,
			"KAS-ECC|Component|1.0" => 86,
			"KAS-FFC||1.0" => 71,
			"KAS-FFC|Component|1.0" => 85,
			"KDF||1.0" => 80,
			"kdf-components|ansix9.63|1.0" => 79,
			"kdf-components|ikev1|1.0" => 73,
			"kdf-components|ikev2|1.0" => 74,
			"kdf-components|snmp|1.0" => 76,
			"kdf-components|srtp|1.0" => 77,
			"kdf-components|ssh|1.0" => 75,
			"kdf-components|tls|1.0" => 78,
			"RSA|decryptionPrimitive|1.0" => 81,
			"RSA|keyGen|1.0" => 58,
			"RSA|keyGen|186-2" => 118,
			"RSA|legacySigVer|1.0" => 83,
			"RSA|sigGen|1.0" => 59,
			"RSA|sigGen|186-2" => 119,
			"RSA|signaturePrimitive|1.0" => 82,
			"RSA|sigVer|1.0" => 60,
			"SHA-1||1.0" => 12,
			"SHA2-224||1.0" => 13,
			"SHA2-256||1.0" => 14,
			"SHA2-384||1.0" => 15,
			"SHA2-512||1.0" => 16,
			"SHA2-512/224||1.0" => 17,
			"SHA2-512/256||1.0" => 18,
			"SHA3-224||1.0" => 34,
			"SHA3-256||1.0" => 35,
			"SHA3-384||1.0" => 36,
			"SHA3-512||1.0" => 37,
			"SHAKE-128||1.0" => 38,
			"SHAKE-256||1.0" => 39,
			"ACVP-TDES-CBC||1.0" => 9,
			"ACVP-TDES-CBCI||1.0" => 55,
			"ACVP-TDES-CFB1||1.0" => 44,
			"ACVP-TDES-CFB64||1.0" => 46,
			"ACVP-TDES-CFB8||1.0" => 45,
			"ACVP-TDES-CFBP1||1.0" => 50,
			"ACVP-TDES-CFBP64||1.0" => 52,
			"ACVP-TDES-CFBP8||1.0" => 51,
			"ACVP-TDES-CTR||1.0" => 54,
			"ACVP-TDES-ECB||1.0" => 10,
			"ACVP-TDES-KW||1.0" => 21,
			"ACVP-TDES-OFB||1.0" => 22,
			"ACVP-TDES-OFBI||1.0" => 56,
			_ => -1,
		};

		private string GetFamily() => AlgorithmID switch
		{
			0 => "AES",
			1 => "AES",
			2 => "AES",
			3 => "AES",
			4 => "AES",
			5 => "AES",
			6 => "AES",
			7 => "AES",
			20 => "AES",
			42 => "AES",
			47 => "AES",
			48 => "AES",
			49 => "AES",
			53 => "AES",
			73 => "Component",
			74 => "Component",
			75 => "Component",
			76 => "Component",
			77 => "Component",
			78 => "Component",
			79 => "Component",
			81 => "Component",
			82 => "Component",
			84 => "Component",
			85 => "Component",
			86 => "Component",
			19 => "DRBG",
			61 => "DRBG",
			62 => "DRBG",
			107 => "DRBG",
			57 => "DSA",
			63 => "DSA",
			64 => "DSA",
			65 => "DSA",
			66 => "DSA",
			108 => "DSA",
			109 => "DSA",
			110 => "DSA",
			111 => "DSA",
			112 => "DSA",
			113 => "DSA",
			67 => "ECDSA",
			68 => "ECDSA",
			69 => "ECDSA",
			70 => "ECDSA",
			114 => "ECDSA",
			115 => "ECDSA",
			116 => "ECDSA",
			117 => "ECDSA",
			23 => "HMAC",
			24 => "HMAC",
			25 => "HMAC",
			26 => "HMAC",
			27 => "HMAC",
			28 => "HMAC",
			29 => "HMAC",
			30 => "HMAC",
			31 => "HMAC",
			32 => "HMAC",
			33 => "HMAC",
			71 => "KAS",
			72 => "KAS",
			80 => "KDF",
			58 => "RSA",
			59 => "RSA",
			60 => "RSA",
			83 => "RSA",
			118 => "RSA",
			119 => "RSA",
			34 => "SHA-3",
			35 => "SHA-3",
			36 => "SHA-3",
			37 => "SHA-3",
			38 => "SHA-3",
			39 => "SHA-3",
			12 => "SHS",
			13 => "SHS",
			14 => "SHS",
			15 => "SHS",
			16 => "SHS",
			17 => "SHS",
			18 => "SHS",
			9 => "TDES",
			10 => "TDES",
			21 => "TDES",
			22 => "TDES",
			43 => "TDES",
			44 => "TDES",
			45 => "TDES",
			46 => "TDES",
			50 => "TDES",
			51 => "TDES",
			52 => "TDES",
			54 => "TDES",
			55 => "TDES",
			56 => "TDES",
			_ => null,
		};
	}
}
