using System.Collections.Generic;
using System.Numerics;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS_SSC.Sp800_56Br2.Ifc
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public string TestType { get; set; }
        public List<TestCase> Tests { get; set; } = new List<TestCase>();

        public bool IsSample { get; set; }

        public SscIfcScheme Scheme { get; set; }

        public KasMode KasMode => KasMode.NoKdfNoKc;

        public KeyAgreementRole KasRole { get; set; }

        public IfcKeyGenerationMethod KeyGenerationMethod { get; set; }

        [JsonIgnore]
        public PrivateKeyModes PrivateKeyMode =>
            KeyGenerationMethod == IfcKeyGenerationMethod.RsaKpg1_crt ||
            KeyGenerationMethod == IfcKeyGenerationMethod.RsaKpg2_crt
                ? PrivateKeyModes.Crt
                : PrivateKeyModes.Standard;

        public int Modulo { get; set; }

        public HashFunctions HashFunctionZ { get; set; }

        [JsonProperty("fixedPubExp", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BigInteger PublicExponent { get; set; }

        [JsonIgnore]
        public SchemeKeyNonceGenRequirement ServerRequirements =>
            KeyGenerationRequirementsHelper.GetKeyGenerationOptionsForSchemeAndRole(Scheme, KasMode,
                KeyGenerationRequirementsHelper.GetOtherPartyKeyAgreementRole(KasRole),
                KeyConfirmationRole.None, KeyConfirmationDirection.None);

        [JsonIgnore]
        public SchemeKeyNonceGenRequirement IutRequirements =>
            KeyGenerationRequirementsHelper.GetKeyGenerationOptionsForSchemeAndRole(Scheme, KasMode, KasRole,
                KeyConfirmationRole.None, KeyConfirmationDirection.None);

        [JsonIgnore] public ShuffleQueue<KeyPair> ShuffleKeys { get; set; }
    }
}
