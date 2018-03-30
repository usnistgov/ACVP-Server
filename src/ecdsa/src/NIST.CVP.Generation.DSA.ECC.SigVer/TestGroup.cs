using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json.Linq;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Crypto.DSA.ECC.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Generation.DSA.ECC.SigVer.Enums;

namespace NIST.CVP.Generation.DSA.ECC.SigVer
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
            TestGroupId = (int) source.tgId;
            var expandoSource = (ExpandoObject) source;

            ParseDomainParams(expandoSource);
            ParseHashAlg(expandoSource);

            Tests = new List<ITestCase>();
            foreach (var test in source.tests)
            {
                Tests.Add(new TestCase(test));
            }
        }

        public int TestGroupId { get; set; }
        public EccDomainParameters DomainParameters { get; set; }
        public HashFunction HashAlg { get; set; }

        public ITestCaseExpectationProvider<SigFailureReasons> TestCaseExpectationProvider { get; set; }

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

        private void ParseDomainParams(ExpandoObject source)
        {
            var curveFactory = new EccCurveFactory();

            if (source.ContainsProperty("curve"))
            {
                var curveName = source.GetTypeFromProperty<string>("curve");
                var curveEnum = EnumHelpers.GetEnumFromEnumDescription<Curve>(curveName);
                var curve = curveFactory.GetCurve(curveEnum);

                DomainParameters = new EccDomainParameters(curve);
            }
        }

        private void ParseHashAlg(ExpandoObject source)
        {
            if (source.ContainsProperty("hashAlg"))
            {
                var shaName = source.GetTypeFromProperty<string>("hashAlg");
                var shaAttributes = AlgorithmSpecificationToDomainMapping.GetMappingFromAlgorithm(shaName);

                HashAlg = new HashFunction(shaAttributes.shaMode, shaAttributes.shaDigestSize);
            }
        }
    }
}
