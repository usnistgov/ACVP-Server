using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.KAS.SafePrimes.Enums;
using NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3.Enums;
using NIST.CVP.Math;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Generation.KAS.Sp800_56Ar3.Ffc
{
    public class TestGroup : TestGroupBase<TestGroup, TestCase, FfcKeyPair>
    {
        [JsonIgnore] public SafePrime SafePrime
        {
            get
            {
                return DomainParameterGenerationMode switch
                {
                    KasDpGeneration.Ffdhe2048 => SafePrime.Ffdhe2048,
                    KasDpGeneration.Ffdhe3072 => SafePrime.Ffdhe3072,
                    KasDpGeneration.Ffdhe4096 => SafePrime.Ffdhe4096,
                    KasDpGeneration.Ffdhe6144 => SafePrime.Ffdhe6144,
                    KasDpGeneration.Ffdhe8192 => SafePrime.Ffdhe8192,
                    KasDpGeneration.Modp2048 => SafePrime.Modp2048,
                    KasDpGeneration.Modp3072 => SafePrime.Modp3072,
                    KasDpGeneration.Modp4096 => SafePrime.Modp4096,
                    KasDpGeneration.Modp6144 => SafePrime.Modp6144,
                    KasDpGeneration.Modp8192 => SafePrime.Modp8192,
                    _ => SafePrime.None
                };
            }
        }
        
        public int DomainParameterL { get; private set; }
        public int DomainParameterN { get; private set; }
        
        [JsonIgnore] public FfcDomainParameters FfcDomainParameters { get; private set; } = new FfcDomainParameters();
        public override IDsaDomainParameters DomainParameters
        {
            get => FfcDomainParameters;
            set
            {
                var castParams = (FfcDomainParameters) value;
                DomainParameterL = castParams.P.ExactBitString().PadToModulusMsb(32).BitLength;
                DomainParameterN = castParams.Q.ExactBitString().PadToModulusMsb(32).BitLength;

                FfcDomainParameters = castParams;
            }
        }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString P
        {
            get => FfcDomainParameters?.P == 0 ? null : new BitString(FfcDomainParameters.P, DomainParameterL);
            set => FfcDomainParameters.P = value.ToPositiveBigInteger();
        }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString Q
        {
            get => FfcDomainParameters?.Q == 0 ? null : new BitString(FfcDomainParameters.Q, DomainParameterN);
            set => FfcDomainParameters.Q = value.ToPositiveBigInteger();
        }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString G
        {
            get => FfcDomainParameters?.G == 0 ? null : new BitString(FfcDomainParameters.G, DomainParameterL);
            set => FfcDomainParameters.G = value.ToPositiveBigInteger();
        }
    }
}