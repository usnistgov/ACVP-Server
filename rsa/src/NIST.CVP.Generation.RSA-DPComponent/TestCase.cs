using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.RSA_DPComponent
{
    public class TestCase : ITestCase
    {
        public int TestCaseId { get; set; }
        public bool FailureTest { get; set; }
        public bool Deferred { get; set; }

        public List<AlgoArrayResponse> ResultsArray { get; set; } = new List<AlgoArrayResponse>();

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

            var expandoSource = (ExpandoObject) source;
            if (expandoSource.ContainsProperty("resultsArray") || expandoSource.ContainsProperty("promptArray"))
            {
                var n = expandoSource.GetBigIntegerFromProperty("n");
                var e = expandoSource.GetBigIntegerFromProperty("e");
                var key = new KeyPair{PubKey = new PublicKey {E = e, N = n}};
                bool failureTest;

                if (expandoSource.ContainsProperty("isSuccess"))
                {
                    // Negate it for 'FailureTest'
                    failureTest = !expandoSource.GetTypeFromProperty<bool>("isSuccess");
                }
                else
                {
                    failureTest = false;
                }

                var algoArray = new AlgoArrayResponse
                {
                    CipherText = expandoSource.GetBitStringFromProperty("cipherText"),
                    PlainText = expandoSource.GetBitStringFromProperty("plainText"),
                    Key = key,
                    FailureTest = failureTest
                };

                ResultsArray.Add(algoArray);
            }
        }

        public bool Merge(ITestCase otherTest)
        {
            if (TestCaseId != otherTest.TestCaseId)
            {
                return false;
            }

            return true;
        }
    }
}
