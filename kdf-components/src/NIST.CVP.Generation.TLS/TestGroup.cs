using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json.Linq;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Crypto.Common.KDF.Components.TLS.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.TLS
{
    public class TestGroup : ITestGroup
    {
        public int TestGroupId { get; set; }
        public HashFunction HashAlg { get; set; }
        public TlsModes TlsMode { get; set; }
        public int KeyBlockLength { get; set; }
        public int PreMasterSecretLength { get; set; }

        public string TestType { get; set; }
        public List<ITestCase> Tests { get; set; }

        public TestGroup()
        {
            Tests = new List<ITestCase>();
        }

        public TestGroup(JObject source) : this(source.ToObject<ExpandoObject>()) { }

        public TestGroup(dynamic source)
        {
            var expandoSource = (ExpandoObject) source;
            TestGroupId = expandoSource.GetTypeFromProperty<int>("tgId");
            TlsMode = EnumHelpers.GetEnumFromEnumDescription<TlsModes>(expandoSource.GetTypeFromProperty<string>("tlsVersion"), false);

            var hashValue = expandoSource.GetTypeFromProperty<string>("hashAlg");
            if (!string.IsNullOrEmpty(hashValue))
            {
                HashAlg = ShaAttributes.GetHashFunctionFromName(hashValue);
            }

            KeyBlockLength = expandoSource.GetTypeFromProperty<int>("keyBlockLength");
            PreMasterSecretLength = expandoSource.GetTypeFromProperty<int>("preMasterSecretLength");

            Tests = new List<ITestCase>();
            foreach (var test in source.tests)
            {
                Tests.Add(new TestCase(test));
            }
        }

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "hashalg":
                    HashAlg = ShaAttributes.GetHashFunctionFromName(value);
                    return true;

                case "pre-master secret length":
                    PreMasterSecretLength = int.Parse(value);
                    return true;

                case "key block length":
                    KeyBlockLength = int.Parse(value);
                    return true;

                case "tlsversion":
                    TlsMode = EnumHelpers.GetEnumFromEnumDescription<TlsModes>(value);
                    return true;
            }

            return false;
        }
    }
}
