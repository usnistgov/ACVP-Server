using Newtonsoft.Json;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KAS.Sp800_56Ar3
{
    public abstract class TestCaseBase<TTestGroup, TTestCase> : ITestCase<TTestGroup, TTestCase>
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase>
        where TTestCase : TestCaseBase<TTestGroup, TTestCase>
    {
        public int TestCaseId { get; set; }
        public TTestGroup ParentGroup { get; set; }
        public bool? TestPassed { get; set; }
        public bool Deferred { get; set; }
        
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
        
        public BitString MacKey { get; set; }
        public BitString MacData { get; set; }
        public BitString Dkm { get; set; }
        public BitString Tag { get; set; }
    }
}