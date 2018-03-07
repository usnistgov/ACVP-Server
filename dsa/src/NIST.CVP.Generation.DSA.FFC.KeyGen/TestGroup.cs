using System.Collections.Generic;
using System.Dynamic;
using System.Numerics;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DSA.FFC.KeyGen
{
    public class TestGroup : ITestGroup
    {
        // Needed for SetString, FireHoseTests
        private BigInteger p;
        private BigInteger q;
        private BigInteger g;

        public int TestGroupId { get; set; }
        public int L { get; set; }
        public int N { get; set; }
        public FfcDomainParameters DomainParams { get; set; }
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
            L = expandoSource.GetTypeFromProperty<int>("l");
            N = expandoSource.GetTypeFromProperty<int>("n");
            
            var p = expandoSource.GetBigIntegerFromProperty("p");
            var q = expandoSource.GetBigIntegerFromProperty("q");
            var g = expandoSource.GetBigIntegerFromProperty("g");
            DomainParams = new FfcDomainParameters(p, q, g);

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
                case "p":
                    p = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "q":
                    q = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "g":
                    g = new BitString(value).ToPositiveBigInteger();
                    DomainParams = new FfcDomainParameters(p, q, g);
                    return true;

                case "l":
                    L = int.Parse(value);
                    return true;

                case "n":
                    N = int.Parse(value);
                    return true;
            }

            return false;
        }
    }
}
