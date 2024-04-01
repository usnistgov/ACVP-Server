using System.Numerics;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.SP800_56Br2.DpComponent
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public TestGroup ParentGroup { get; set; }
        public bool? TestPassed { get; set; }
        public bool Deferred { get; set; }

        [JsonProperty(PropertyName = "ct")]
        public BitString CipherText { get; set; }
        
        [JsonProperty(PropertyName = "pt")]
        public BitString PlainText { get; set; }
        public BigInteger Dmp1 { get; set; }
        public BigInteger Dmq1 { get; set; }
        public BigInteger Iqmp { get; set; }
        public BigInteger P { get; set; }
        public BigInteger Q { get; set; }
        public BigInteger D { get; set; }
        public BigInteger N { get; set; }
        public BigInteger E { get; set; }
    }
}
