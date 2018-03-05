using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Numerics;
using Newtonsoft.Json.Linq;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.DSA.ECC.SigGen
{
    public class TestGroup : ITestGroup
    {
        public int TestGroupId { get; set; }
        public Curve Curve { get; set; }
        public HashFunction HashAlg { get; set; }
        public EccKeyPair KeyPair { get; set; }
        public bool ComponentTest { get; set; }

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
            Curve = EnumHelpers.GetEnumFromEnumDescription<Curve>(expandoSource.GetTypeFromProperty<string>("curve"), false);
            ComponentTest = expandoSource.GetTypeFromProperty<bool>("componentTest");

            var hashValue = expandoSource.GetTypeFromProperty<string>("hashAlg");
            if (!string.IsNullOrEmpty(hashValue))
            {
                HashAlg = ShaAttributes.GetHashFunctionFromName(hashValue);
            }

            var qx = expandoSource.GetBigIntegerFromProperty("qx");
            var qy = expandoSource.GetBigIntegerFromProperty("qy");
            KeyPair = new EccKeyPair(new EccPoint(qx, qy));

            Tests = new List<ITestCase>();
            foreach (var test in source.tests)
            {
                var tc = new TestCase(test)
                {
                    Parent = this
                };
                Tests.Add(tc);
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
                case "curve":
                    Curve = EnumHelpers.GetEnumFromEnumDescription<Curve>(value);
                    return true;

                case "hashalg":
                    HashAlg = ShaAttributes.GetHashFunctionFromName(value);
                    return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return $"{EnumHelpers.GetEnumDescriptionFromEnum(Curve)}{HashAlg.Name}".GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is TestGroup otherGroup)
            {
                return GetHashCode() == otherGroup.GetHashCode();
            }
            
            return false;
        }
    }
}
