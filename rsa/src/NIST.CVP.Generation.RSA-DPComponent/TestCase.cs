using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
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

        public ITestGroup Parent { get; set; }
        public List<AlgoArrayResponseSignature> ResultsArray { get; set; } = new List<AlgoArrayResponseSignature>();

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
            if (expandoSource.ContainsProperty("resultsArray"))
            {
                foreach (var item in source.resultsArray)
                {
                    var expandoItem = (ExpandoObject) item;
                    var n = expandoItem.GetBigIntegerFromProperty("n");
                    var e = expandoItem.GetBigIntegerFromProperty("e");
                    var key = new KeyPair{PubKey = new PublicKey {E = e, N = n}};
                    bool failureTest;

                    if (expandoItem.ContainsProperty("isSuccess"))
                    {
                        // Negate it for 'FailureTest'
                        failureTest = !expandoItem.GetTypeFromProperty<bool>("isSuccess");
                    }
                    else
                    {
                        failureTest = false;
                    }

                    var algoArray = new AlgoArrayResponseSignature
                    {
                        CipherText = expandoItem.GetBitStringFromProperty("cipherText"),
                        PlainText = expandoItem.GetBitStringFromProperty("plainText"),
                        Key = key,
                        FailureTest = failureTest
                    };

                    ResultsArray.Add(algoArray);
                }
            }
        }

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            if (ResultsArray == null)
            {
                return false;
            }

            var latest = ResultsArray.Last();

            switch (name.ToLower())
            {
                case "c":
                    latest.CipherText = new BitString(value);
                    return true;

                case "k":
                    latest.PlainText = new BitString(value);
                    return true;

                case "n":
                    latest.Key = new KeyPair {PrivKey = new PrivateKey(), PubKey = new PublicKey()};
                    latest.Key.PubKey.N = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "e":
                    latest.Key.PubKey.E = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "result":
                    latest.FailureTest = (value == "fail");
                    return true;
            }

            return false;
        }
    }
}
