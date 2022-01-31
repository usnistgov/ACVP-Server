using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.RSA.PrimeGenerators;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA_SigVer.IntegrationTests.Fips186_4
{
    [TestFixture]
    public class FailingScenarioTest
    {
        [Test]
        public void ShouldGeneratePrimes()
        {
            //            "KeyMode": "probable",
            //            "PublicExponentMode": "random",
            //            "PublicExponent": {
            //                "Value": "8B00DEF94C12EB",
            //                "BitLength": 56
            //            },
            //            "Modulus": 1024,
            //            "HashAlg": null,
            //            "PrimeTest": "2pow100",
            //            "Seed": {
            //                "Value": "743735CE632183ADDF8A23A46DF33138B93F2091",
            //                "BitLength": 160
            //            },
            //            "BitLens": [
            //            103,
            //            111,
            //            121,
            //            110
            //                ],
            //            "Standard": "FIPS186-4",
            //            "KeyFormat": "standard",
            //            "PMod8": 0,
            //            "QMod8": 0

            var param = new RsaKeyParameters()
            {
                KeyMode = PrimeGenModes.RandomProbablePrimes,
                PublicExponentMode = PublicExponentModes.Random,
                PublicExponent = new BitString("8B00DEF94C12EB"),
                Modulus = 2048,
                HashAlg = null,
                PrimeTest = PrimeTestModes.TwoPow100ErrorBound,
                Seed = new BitString("743735CE632183ADDF8A23A46DF33138B93F2091"),
                BitLens = new[] { 103, 111, 121, 110 },
                Standard = Fips186Standard.Fips186_4,
                KeyFormat = PrivateKeyModes.Standard,
                PMod8 = 0,
                QMod8 = 0
            };

            ISha sha = null;
            if (param.HashAlg != null)
            {
                sha = new NativeShaFactory().GetShaInstance(param.HashAlg);
            }

            var keyResult = new KeyBuilder(new PrimeGeneratorFactory())
                .WithBitlens(param.BitLens)
                .WithEntropyProvider(new EntropyProvider(new Random800_90()))
                .WithHashFunction(sha)
                .WithNlen(param.Modulus)
                .WithPrimeGenMode(param.KeyMode)
                .WithPrimeTestMode(param.PrimeTest)
                .WithPublicExponent(param.PublicExponent)
                .WithKeyComposer(new RsaKeyComposer())
                .WithSeed(param.Seed)
                .WithStandard(param.Standard)
                .WithPMod8(param.PMod8)
                .WithQMod8(param.QMod8)
                .Build();

            Assert.IsTrue(keyResult.Success);
        }
    }
}
