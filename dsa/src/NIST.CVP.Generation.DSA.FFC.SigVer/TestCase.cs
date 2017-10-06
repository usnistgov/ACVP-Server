using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.DSA.FFC.Enums;
using NIST.CVP.Crypto.DSA.FFC.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Generation.DSA.FFC.SigVer.FailureHandlers;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DSA.FFC.SigVer
{
    public class TestCase : ITestCase
    {
        public int TestCaseId { get; set; }
        public bool FailureTest { get; set; }
        public bool Deferred { get; set; }

        public bool Result { get; set; }
        public IFailureReason<SigFailureReasons> Reason { get; set; }
        public FfcKeyPair Key { get; set; }
        public BitString Message { get; set; }
        public FfcSignature Signature { get; set; }

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
            
            if (((ExpandoObject)source).ContainsProperty("result"))
            {
                Result = (((string)source.result).ToLower() == "passed");
            }

            if (((ExpandoObject)source).ContainsProperty("reason"))
            {
                var reason = EnumHelper.StringToSigFailureReason((string)source.reason);
                Reason = new FailureReason(reason);
            }
        }

        public bool Merge(ITestCase otherTest)
        {
            // We don't need any properties from the prompt...
            return (TestCaseId == otherTest.TestCaseId);
        }
    }
}
