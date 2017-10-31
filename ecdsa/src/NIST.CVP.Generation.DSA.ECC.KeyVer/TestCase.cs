using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Generation.Core.Helpers;
using NIST.CVP.Generation.DSA.ECC.KeyVer.Enums;

namespace NIST.CVP.Generation.DSA.ECC.KeyVer
{
    public class TestCase : ITestCase
    {
        public int TestCaseId { get; set; }
        public bool FailureTest { get; set; }
        public bool Deferred { get; set; }
        public TestCaseExpectationEnum Reason { get; set; }
        public bool Result { get; set; }

        public EccKeyPair KeyPair { get; set; }

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

        private void MapToProperties(dynamic source)
        {
            TestCaseId = (int)source.tcId;

            ExpandoObject expandoSource = (ExpandoObject)source;

            var resultValue = expandoSource.GetTypeFromProperty<string>("result");
            Result = Disposition.Passed == EnumHelpers.GetEnumFromEnumDescription<Disposition>(resultValue, false);

            var reasonValue = expandoSource.GetTypeFromProperty<string>("reason");
            Reason = EnumHelpers.GetEnumFromEnumDescription<TestCaseExpectationEnum>(reasonValue, false);
        }

        public bool Merge(ITestCase otherTest)
        {
            // We don't need any properties from the prompt...
            return (TestCaseId == otherTest.TestCaseId);
        }
    }
}
