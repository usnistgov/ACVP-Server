using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json.Linq;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Crypto.DSA.ECC.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Generation.DSA.ECC.SigVer.Enums;

namespace NIST.CVP.Generation.DSA.ECC.SigVer
{
    public class TestGroup : ITestGroup
    {
        public int TestGroupId { get; set; }
        public EccDomainParameters DomainParameters { get; set; }
        public HashFunction HashAlg { get; set; }

        public ITestCaseExpectationProvider<SigFailureReasons> TestCaseExpectationProvider { get; set; }

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
            var curveName = expandoSource.GetTypeFromProperty<string>("curve");

            var curve = EnumHelpers.GetEnumFromEnumDescription<Curve>(curveName, false);
            var curveFactory = new EccCurveFactory();

            DomainParameters = new EccDomainParameters(curveFactory.GetCurve(curve));

            var hashValue = expandoSource.GetTypeFromProperty<string>("hashAlg");
            if (!string.IsNullOrEmpty(hashValue))
            {
                HashAlg = ShaAttributes.GetHashFunctionFromName(hashValue);
            }

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
                case "curve":
                    var factory = new EccCurveFactory();
                    var curve = factory.GetCurve(EnumHelpers.GetEnumFromEnumDescription<Curve>(value));
                    DomainParameters = new EccDomainParameters(curve);
                    return true;

                case "hashalg":
                    var shaAttributes = AlgorithmSpecificationToDomainMapping.GetMappingFromAlgorithm(value);
                    HashAlg = new HashFunction(shaAttributes.shaMode, shaAttributes.shaDigestSize);
                    return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return ($"{EnumHelpers.GetEnumDescriptionFromEnum(DomainParameters.CurveE.CurveName)}{HashAlg.Name}").GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var otherGroup = obj as TestGroup;
            if (otherGroup == null)
            {
                return false;
            }
            return this.GetHashCode() == otherGroup.GetHashCode();
        }
    }
}
