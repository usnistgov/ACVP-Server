using System;
using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3.Enums;
using NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3.Helpers;

namespace NIST.CVP.Generation.KAS.Sp800_56Ar3.Ecc
{
    public class TestGroup : TestGroupBase<TestGroup, TestCase, EccKeyPair>
    {
        [JsonIgnore] public Curve Curve => KasEnumMapping.GetCurveFromKasDpGeneration(DomainParameterGenerationMode);
    }
}