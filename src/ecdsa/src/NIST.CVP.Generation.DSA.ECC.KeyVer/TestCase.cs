using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Numerics;
using System.Text;
using Newtonsoft.Json.Linq;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Generation.DSA.ECC.KeyVer.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DSA.ECC.KeyVer
{
    public class TestCase : ITestCase
    {
        private BigInteger _setStringQx;
        private BigInteger _setStringQy;

        public TestCase() { }

        public TestCase(JObject source)
        {
            var data = source.ToObject<ExpandoObject>();
            MapToProperties(data);
        }

        public TestCase(dynamic source)
        {
            MapToProperties(source);
        }

        public int TestCaseId { get; set; }
        public bool FailureTest { get; set; }
        public bool Deferred { get; set; }
        public TestCaseExpectationEnum Reason { get; set; }
        public bool Result { get; set; }

        public EccKeyPair KeyPair { get; set; }

        private void MapToProperties(dynamic source)
        {
            TestCaseId = (int)source.tcId;

            ExpandoObject expandoSource = (ExpandoObject)source;

            var resultValue = expandoSource.GetTypeFromProperty<string>("result");
            Result = Disposition.Passed == EnumHelpers.GetEnumFromEnumDescription<Disposition>(resultValue, false);

            var reasonValue = expandoSource.GetTypeFromProperty<string>("reason");
            Reason = EnumHelpers.GetEnumFromEnumDescription<TestCaseExpectationEnum>(reasonValue, false);
        }

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            // Sometimes these values aren't even length...
            if (value.Length % 2 != 0)
            {
                value = value.Insert(0, "0");
            }

            switch (name.ToLower())
            {
                case "qx":
                    _setStringQx = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "qy":
                    _setStringQy = new BitString(value).ToPositiveBigInteger();
                    KeyPair = new EccKeyPair(new EccPoint(_setStringQx, _setStringQy));
                    return true;

                case "result":
                    Result = value.Contains("p (0");
                    return true;
            }

            return false;
        }
    }
}
