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
    public abstract class TestGroupBase<TTestGroup, TTestCase> : ITestGroup<TTestGroup, TTestCase>
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase>
        where TTestCase : TestCaseBase<TTestGroup, TTestCase>
    {
        public int TestGroupId { get; set; }
        public string TestType { get; set; }
        public List<TTestCase> Tests { get; set; } = new List<TTestCase>();
        
        public bool IsSample { get; set; }
        
        public KasAssurance Function { get; set; }

        public KasDpGeneration DomainParameterGenerationMode { get; set; }
        
        public IDsaDomainParameters DomainParameters { get; set; }
        
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
        
        public IKdfConfiguration KdfConfiguration { get; set; }
        public MacConfiguration MacConfiguration { get; set; }
        public KeyConfirmationDirection KeyConfirmationDirection { get; set; }
        public KeyConfirmationRole KeyConfirmationRole { get; set; }
    }
}