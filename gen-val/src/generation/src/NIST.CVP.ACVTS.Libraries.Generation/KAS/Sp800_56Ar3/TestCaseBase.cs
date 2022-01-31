using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS.Sp800_56Ar3
{
    public abstract class TestCaseBase<TTestGroup, TTestCase, TKeyPair> : ITestCase<TTestGroup, TTestCase>
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase, TKeyPair>
        where TTestCase : TestCaseBase<TTestGroup, TTestCase, TKeyPair>
        where TKeyPair : IDsaKeyPair
    {
        public int TestCaseId { get; set; }
        public TTestGroup ParentGroup { get; set; }
        public bool? TestPassed { get; set; }
        public bool Deferred { get; set; }

        [JsonIgnore] public abstract TKeyPair StaticKeyServer { get; set; }
        [JsonIgnore] public abstract TKeyPair EphemeralKeyServer { get; set; }
        [JsonIgnore] public abstract TKeyPair StaticKeyIut { get; set; }
        [JsonIgnore] public abstract TKeyPair EphemeralKeyIut { get; set; }

        /// <summary>
        /// The intended result of the test case
        /// </summary>
        public KasValTestDisposition TestCaseDisposition { get; set; }

        /// <summary>
        /// The ephemeral nonce used from the server perspective.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString EphemeralNonceServer { get; set; }
        /// <summary>
        /// The DKM nonce used from the server perspective.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString DkmNonceServer { get; set; }

        /// <summary>
        /// The ephemeral nonce used from the IUT perspective. 
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString EphemeralNonceIut { get; set; }
        /// <summary>
        /// The DKM nonce used from the iut perspective.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString DkmNonceIut { get; set; }

        /// <summary>
        /// The KDF parameters used in the KDF function.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IKdfParameter KdfParameter { get; set; }

        public BitString Z { get; set; }
        public BitString MacKey { get; set; }
        public BitString MacData { get; set; }
        public BitString Dkm { get; set; }
        public BitString Tag { get; set; }
    }
}
