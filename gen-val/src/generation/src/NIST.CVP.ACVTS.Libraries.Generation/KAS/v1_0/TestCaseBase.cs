using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS.v1_0
{
    public abstract class TestCaseBase<TTestGroup, TTestCase, TKasAlgoAttributes> : ITestCase<TTestGroup, TTestCase>
        where TKasAlgoAttributes : IKasAlgoAttributes
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase, TKasAlgoAttributes>
        where TTestCase : TestCaseBase<TTestGroup, TTestCase, TKasAlgoAttributes>
    {
        public int TestCaseId { get; set; }
        public bool? TestPassed { get; set; }
        public bool Deferred { get; set; }
        public TTestGroup ParentGroup { get; set; }

        public KasValTestDisposition TestCaseDisposition { get; set; }

        [JsonProperty(PropertyName = "nonceDkmServer")]
        public BitString DkmNonceServer { get; set; }

        [JsonProperty(PropertyName = "nonceEphemeralServer")]
        public BitString EphemeralNonceServer { get; set; }


        [JsonProperty(PropertyName = "nonceDkmIut")]
        public BitString DkmNonceIut { get; set; }

        [JsonProperty(PropertyName = "nonceEphemeralIut")]
        public BitString EphemeralNonceIut { get; set; }


        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int IdIutLen { get; set; }

        public BitString IdIut { get; set; }


        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int OiLen { get; set; }

        [JsonProperty(PropertyName = "oi")]
        public BitString OtherInfo { get; set; }

        public BitString NonceNoKc { get; set; }

        public BitString NonceAesCcm { get; set; }


        public BitString Z { get; set; }

        public BitString Dkm { get; set; }

        public BitString MacData { get; set; }

        [JsonProperty(PropertyName = "hashZIut")]
        public BitString HashZ { get; set; }

        [JsonProperty(PropertyName = "tagIut")]
        public BitString Tag { get; set; }
    }
}
