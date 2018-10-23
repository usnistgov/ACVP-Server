using NIST.CVP.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using System.Collections.Generic;
using System.Linq;

namespace NIST.CVP.Generation.RSA_DPComponent
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public TestGroup ParentGroup { get; set; }
        public bool? TestPassed { get; set; }
        public bool Deferred { get; set; }

        public List<AlgoArrayResponseSignature> ResultsArray { get; set; }

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
                    latest.TestPassed = value != "fail";
                    return true;
            }

            return false;
        }
    }
}
