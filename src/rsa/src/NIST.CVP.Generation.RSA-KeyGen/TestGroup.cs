using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json.Linq;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.RSA_KeyGen
{
    public class TestGroup : ITestGroup
    {
        public TestGroup()
        {
            Tests = new List<ITestCase>();
        }

        public TestGroup(JObject source) : this(source.ToObject<ExpandoObject>()) { }

        public TestGroup(dynamic source)
        {
            TestType = source.testType;

            var expandoSource = (ExpandoObject) source;

            TestGroupId = expandoSource.GetTypeFromProperty<int>("tgId");
            Modulo = expandoSource.GetTypeFromProperty<int>("modulo");
            InfoGeneratedByServer = expandoSource.GetTypeFromProperty<bool>("infoGeneratedByServer");
            PrimeGenMode = EnumHelpers.GetEnumFromEnumDescription<PrimeGenModes>(expandoSource.GetTypeFromProperty<string>("randPQ"));

            var hashAlgName = expandoSource.GetTypeFromProperty<string>("hashAlg");
            if (!string.IsNullOrEmpty(hashAlgName))
            {
                HashAlg = ShaAttributes.GetHashFunctionFromName(hashAlgName);
            }

            PrimeTest = EnumHelpers.GetEnumFromEnumDescription<PrimeTestModes>(expandoSource.GetTypeFromProperty<string>("primeTest"), false);
            PubExp = EnumHelpers.GetEnumFromEnumDescription<PublicExponentModes>(expandoSource.GetTypeFromProperty<string>("pubExpMode"));
            FixedPubExp = expandoSource.GetBitStringFromProperty("fixedPubExp");
            KeyFormat = EnumHelpers.GetEnumFromEnumDescription<PrivateKeyModes>(expandoSource.GetTypeFromProperty<string>("keyFormat"));

            Tests = new List<ITestCase>();
            foreach (var test in source.tests)
            {
                Tests.Add(new TestCase(test));
            }
        }

        public int TestGroupId { get; set; }
        public bool InfoGeneratedByServer { get; set; }
        public int Modulo { get; set; }
        public BitString FixedPubExp { get; set; }
        public List<ITestCase> Tests { get; set; }
        public string TestType { get; set; }

        public HashFunction HashAlg { get; set; }
        public PrivateKeyModes KeyFormat { get; set; }
        public PrimeTestModes PrimeTest { get; set; }
        public PrimeGenModes PrimeGenMode { get; set; }
        public PublicExponentModes PubExp { get; set; }

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(value))
            {
                return false;
            }

            try
            {
                switch (name.ToLower())
                {
                    case "primemethod":
                        PrimeGenMode = EnumHelpers.GetEnumFromEnumDescription<PrimeGenModes>(value);
                        return true;
                    case "mod":
                        Modulo = int.Parse(value);
                        return true;
                    case "hash":
                        HashAlg = ShaAttributes.GetHashFunctionFromName(value);
                        return true;
                    case "table for m-t test":
                        PrimeTest = EnumHelpers.GetEnumFromEnumDescription<PrimeTestModes>(value);
                        return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
