using System.Collections.Generic;
using System.Numerics;
using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KTS;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KAS_IFC.v1_0
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public string TestType { get; set; }
        public List<TestCase> Tests { get; set; } = new List<TestCase>();
        
        public IfcScheme Scheme { get; set; }
        
        public KasMode KasMode { get; set; }
        
        public KeyAgreementRole KasRole { get; set; }
        
        public IfcKeyGenerationMethod KeyGenerationMethod { get; set; }
        
        public int Modulo { get; set; }
        
        [JsonIgnore]
        public BigInteger PublicExponent { get; set; }
        
        public int L { get; set; }
        
        public BitString IutId { get; set; }
        public BitString ServerId { get; set; } = new BitString("434156536964");
        
        /// <summary>
        /// Public keys supplied by the IUT, for use in AFT tests.
        /// </summary>
        public KeyPair[] IutKeys { get; set; }
        
        public IKasKdfConfiguration KdfConfiguration { get; set; }
        public KtsConfiguration KtsConfiguration { get; set; }
        public MacConfiguration MacConfiguration { get; set; }
        public KeyConfirmationDirection KeyConfirmationDirection { get; set; }
        public KeyConfirmationRole KeyConfirmationRole { get; set; }
    }
}