using System.Collections.Generic;
using System.Linq;
using ACVPCore.Algorithms.Persisted;
using LCAVPCore.Registration.MathDomain;
using Newtonsoft.Json;

namespace LCAVPCore.Registration.Algorithms.Component
{
	public class ANS_963 : AlgorithmBase, IAlgorithm
	{
		[JsonProperty(PropertyName = "hashAlg")]
		public List<string> HashAlgorithms { get; private set; } = new List<string>();

		[JsonProperty(PropertyName = "fieldSize")]
		public List<int> FieldSize { get; private set; } = new List<int>();

		[JsonProperty(PropertyName = "sharedInfoLength")]
		public Domain SharedInfoLength { get; private set; } = new Domain();

		[JsonProperty(PropertyName = "keyDataLength")]
		public Domain KeyDataLength { get; private set; } = new Domain();

		public ANS_963(Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "kdf-components", "ansix9.63")
		{
			//Prereqs
			PreReqs.Add(BuildPrereq("SHS", options.GetValue("KDF_800_135_Prerequisite_SHA")));

			if (options.GetValue("KDF_800_135_ANSX963_2001_SHA_224") == "True") HashAlgorithms.Add("SHA2-224");
			if (options.GetValue("KDF_800_135_ANSX963_2001_SHA_256") == "True") HashAlgorithms.Add("SHA2-256");
			if (options.GetValue("KDF_800_135_ANSX963_2001_SHA_384") == "True") HashAlgorithms.Add("SHA2-384");
			if (options.GetValue("KDF_800_135_ANSX963_2001_SHA_512") == "True") HashAlgorithms.Add("SHA2-512");

			//Field size requires interpolating the min/max and including all the allowed values
			int[] allowedValues = { 224, 233, 256, 283, 384, 409, 521, 571 };
			int minValue = ParsingHelper.ParseValueToInteger(options.GetValue("KDF_800_135_ANSX963_2001_Z_length1"));
			int maxValue = ParsingHelper.ParseValueToInteger(options.GetValue("KDF_800_135_ANSX963_2001_Z_length2"));

			FieldSize = allowedValues.Where(x => x >= minValue && x <= maxValue).ToList();

			int minSharedInfoLength = ParsingHelper.ParseValueToInteger(options.GetValue("KDF_800_135_ANSX963_2001_OtherInfo_length1"));
			int maxSharedInfoLength = ParsingHelper.ParseValueToInteger(options.GetValue("KDF_800_135_ANSX963_2001_OtherInfo_length2"));

			if (minSharedInfoLength == maxSharedInfoLength)
			{
				SharedInfoLength.Add(minSharedInfoLength);
			}
			else {
				SharedInfoLength.Add(new Range
				{
					Min = minSharedInfoLength,
					Max = maxSharedInfoLength
				});
			}

			int minKeyDataLength = ParsingHelper.ParseValueToInteger(options.GetValue("KDF_800_135_ANSX963_2001_keydata_length1"));
			int maxKeyDataLength = ParsingHelper.ParseValueToInteger(options.GetValue("KDF_800_135_ANSX963_2001_keydata_length2"));

			if (minKeyDataLength == maxKeyDataLength)
			{
				KeyDataLength.Add(minKeyDataLength);
			}
			else {
				KeyDataLength.Add(new Range
				{
					Min = minKeyDataLength,
					Max = maxKeyDataLength
				});
			}
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new ACVPCore.Algorithms.Persisted.KDF_ANSIX963
		{
			HashAlgorithms = HashAlgorithms,
			FieldSizes = FieldSize,
			SharedInfoLength = SharedInfoLength.ToCoreDomain(),
			KeyDataLength = KeyDataLength.ToCoreDomain()
		};
	}
}
