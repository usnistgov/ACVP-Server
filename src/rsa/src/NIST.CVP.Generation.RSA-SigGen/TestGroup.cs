using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.RSA_SigGen
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
            Mode = EnumHelpers.GetEnumFromEnumDescription<SignatureSchemes>(expandoSource.GetTypeFromProperty<string>("sigType"));
            Modulo = expandoSource.GetTypeFromProperty<int>("modulo");
            HashAlg = ShaAttributes.GetHashFunctionFromName(expandoSource.GetTypeFromProperty<string>("hashAlg"));
            SaltLen = expandoSource.GetTypeFromProperty<int>("saltLen");

            var e = expandoSource.GetBigIntegerFromProperty("e");
            var n = expandoSource.GetBigIntegerFromProperty("n");

            // TODO: do we need d here?
            Key = new KeyPair {PubKey = new PublicKey {E = e, N = n}};

            Tests = new List<ITestCase>();
            foreach (var test in source.tests)
            {
                Tests.Add(new TestCase(test));
            }
        }

        public int TestGroupId { get; set; }
        public SignatureSchemes Mode { get; set; }
        public int Modulo { get; set; }
        public HashFunction HashAlg { get; set; }
        public int SaltLen { get; set; }
        public KeyPair Key { get; set; }

        public string TestType { get; set; }
        public List<ITestCase> Tests { get; set; }

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "mod":
                    Modulo = int.Parse(value);
                    return true;

                case "n":
                    if (Key == null) { Key = new KeyPair {PubKey = new PublicKey()}; }
                    Key.PubKey.N = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "e":
                    Key.PubKey.E = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "d":
                    var d = new BitString(value).ToPositiveBigInteger();
                    Key.PrivKey = new PrivateKey {D = d};
                    return true;

                case "hash":
                    HashAlg = ShaAttributes.GetHashFunctionFromName(value);
                    return true;

                case "saltlen":
                    SaltLen = int.Parse(value);
                    return true;
            }

            return false;
        }
    }
}
