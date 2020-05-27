using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3.Enums;
using NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.KAS.Sp800_56Ar3.Enums;

namespace NIST.CVP.Generation.KAS_SSC.Sp800_56Ar3
{
	public abstract class TestGroupBase<TTestGroup, TTestCase, TKeyPair> : ITestGroup<TTestGroup, TTestCase>
		where TTestGroup : TestGroupBase<TTestGroup, TTestCase, TKeyPair>, new()
		where TTestCase : TestCaseBase<TTestGroup, TTestCase, TKeyPair>, new()
		where TKeyPair : IDsaKeyPair
	{
		public int TestGroupId { get; set; }
		public string TestType { get; set; }
		public List<TTestCase> Tests { get; set; } = new List<TTestCase>();
		
		public KasDpGeneration DomainParameterGenerationMode { get; set; }
        
		[JsonIgnore] public virtual IDsaDomainParameters DomainParameters { get; set; }
        
		public KasScheme Scheme { get; set; }
        
		public KasAlgorithm KasAlgorithm { get; set; }
        
		public KeyAgreementRole KasRole { get; set; }

		public KasMode KasMode { get; set; }
		
		[JsonIgnore]
		public SchemeKeyNonceGenRequirement KeyNonceGenRequirementsIut => 
			KasEnumMapping.GetSchemeRequirements(Scheme, KasMode, KasRole, KeyConfirmationRole.None, KeyConfirmationDirection.None).requirments;
		[JsonIgnore]
		public SchemeKeyNonceGenRequirement KeyNonceGenRequirementsServer => 
			KasEnumMapping.GetSchemeRequirements(Scheme, KasMode, 
				KeyGenerationRequirementsHelper.GetOtherPartyKeyAgreementRole(KasRole), 
				KeyGenerationRequirementsHelper.GetOtherPartyKeyConfirmationRole(KeyConfirmationRole.None),  
				KeyConfirmationDirection.None).requirments;
	}
}