using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS_SSC.Sp800_56Ar3
{
    public abstract class TestGroupBase<TTestGroup, TTestCase, TKeyPair> : ITestGroup<TTestGroup, TTestCase>
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase, TKeyPair>, new()
        where TTestCase : TestCaseBase<TTestGroup, TTestCase, TKeyPair>, new()
        where TKeyPair : IDsaKeyPair
    {
        public int TestGroupId { get; set; }
        public string TestType { get; set; }
        public List<TTestCase> Tests { get; set; } = new List<TTestCase>();

        public bool IsSample { get; set; }

        public KasDpGeneration DomainParameterGenerationMode { get; set; }

        [JsonIgnore] public virtual IDsaDomainParameters DomainParameters { get; set; }
        [JsonIgnore] public ShuffleQueue<TKeyPair> ShuffleKeys { get; set; }

        public KasScheme Scheme { get; set; }

        public KasAlgorithm KasAlgorithm { get; set; }

        public KeyAgreementRole KasRole { get; set; }

        public KasMode KasMode { get; set; }

        public HashFunctions HashFunctionZ { get; set; }

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
