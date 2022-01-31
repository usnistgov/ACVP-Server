using System;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3.Enums;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS_SSC.Sp800_56Ar3.Ecc
{
    public class TestGroup : TestGroupBase<TestGroup, TestCase, EccKeyPair>
    {
        [JsonIgnore]
        public Curve Curve
        {
            get
            {
                return DomainParameterGenerationMode switch
                {
                    KasDpGeneration.P192 => Curve.P192,
                    KasDpGeneration.P224 => Curve.P224,
                    KasDpGeneration.P256 => Curve.P256,
                    KasDpGeneration.P384 => Curve.P384,
                    KasDpGeneration.P521 => Curve.P521,
                    KasDpGeneration.K163 => Curve.K163,
                    KasDpGeneration.K233 => Curve.K233,
                    KasDpGeneration.K283 => Curve.K283,
                    KasDpGeneration.K409 => Curve.K409,
                    KasDpGeneration.K571 => Curve.K571,
                    KasDpGeneration.B163 => Curve.B163,
                    KasDpGeneration.B233 => Curve.B233,
                    KasDpGeneration.B283 => Curve.B283,
                    KasDpGeneration.B409 => Curve.B409,
                    KasDpGeneration.B571 => Curve.B571,
                    _ => throw new ArgumentException(nameof(DomainParameterGenerationMode))
                };
            }
        }
    }
}
