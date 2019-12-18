using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3.Enums;
using NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.KAS.Sp800_56Ar3.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KAS.Sp800_56Ar3
{
    public abstract class TestGroupBase<TTestGroup, TTestCase, TKeyPair> : ITestGroup<TTestGroup, TTestCase>
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase, TKeyPair>
        where TTestCase : TestCaseBase<TTestGroup, TTestCase, TKeyPair>
        where TKeyPair : IDsaKeyPair
    {
        public int TestGroupId { get; set; }
        public string TestType { get; set; }
        public List<TTestCase> Tests { get; set; } = new List<TTestCase>();
        
        public bool IsSample { get; set; }
        
        public KasAssurance Function { get; set; }

        public KasDpGeneration DomainParameterGenerationMode { get; set; }
        
        [JsonIgnore] public IDsaDomainParameters DomainParameters { get; set; }
        
        public KasScheme Scheme { get; set; }
        
        public KasAlgorithm KasAlgorithm { get; set; }
        
        public KeyAgreementRole KasRole { get; set; }

        public KasMode KasMode { get; set; }

        public int L { get; set; }
        
        [JsonIgnore]
        public SchemeKeyNonceGenRequirement KeyNonceGenRequirementsIut => 
            KasEnumMapping.GetSchemeRequirements(Scheme, KasMode, KasRole, KeyConfirmationRole, KeyConfirmationDirection).requirments;
        [JsonIgnore]
        public SchemeKeyNonceGenRequirement KeyNonceGenRequirementsServer => 
            KasEnumMapping.GetSchemeRequirements(Scheme, KasMode, 
                KeyGenerationRequirementsHelper.GetOtherPartyKeyAgreementRole(KasRole), 
                KeyGenerationRequirementsHelper.GetOtherPartyKeyConfirmationRole(KeyConfirmationRole),  
                KeyConfirmationDirection).requirments;
        
        public BitString IutId { get; set; }
        public BitString ServerId { get; } = new BitString("434156536964");
        
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IKdfConfiguration KdfConfiguration { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public MacConfiguration MacConfiguration { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public KeyConfirmationDirection KeyConfirmationDirection { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public KeyConfirmationRole KeyConfirmationRole { get; set; }
    }
}