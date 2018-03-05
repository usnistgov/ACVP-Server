using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.ECC.SigGen.Tests
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorTests
    {
       private static object[] parameters =
       {
            new object[]
            {
                1,
                new ParameterBuilder()
                    .WithCapabilities(new []
                        {
                            ParameterBuilder.GetCapabilityWith(new[] { "p-224" }, new[] { "sha2-224" })
                        })
                    .Build()
            },
            new object[]
            {
                4,
                new ParameterBuilder()
                    .WithCapabilities(new []
                        {
                            ParameterBuilder.GetCapabilityWith(new[] { "b-233" }, new[] { "sha2-512", "sha2-512/224" }),
                            ParameterBuilder.GetCapabilityWith(new[] { "k-571" }, new[] { "sha2-384", "sha2-512/256" })
                        })
                    .Build()
            },
            new object[]
            {
                12 * 6,
                new ParameterBuilder()
                    .WithCapabilities(new []
                        {
                            ParameterBuilder.GetCapabilityWith(ParameterValidator.VALID_CURVES, ParameterValidator.VALID_HASH_ALGS),
                        })
                    .Build()
            },
            new object[]
            {
                12 * 6,
                new ParameterBuilder()
                    .WithCapabilities(new []
                        {
                            ParameterBuilder.GetCapabilityWith(ParameterValidator.VALID_CURVES, ParameterValidator.VALID_HASH_ALGS),
                            ParameterBuilder.GetCapabilityWith(ParameterValidator.VALID_CURVES, ParameterValidator.VALID_HASH_ALGS),
                            ParameterBuilder.GetCapabilityWith(ParameterValidator.VALID_CURVES, ParameterValidator.VALID_HASH_ALGS),
                        })
                    .Build()
            },
            new object[]
            {
                2 + 2 + 2 + 1,
                new ParameterBuilder()
                    .WithCapabilities(new []
                        {
                            ParameterBuilder.GetCapabilityWith(new[] { "b-233" }, new[] { "sha2-512", "sha2-512/224" }),
                            ParameterBuilder.GetCapabilityWith(new[] { "k-571" }, new[] { "sha2-384", "sha2-512/256" }),
                            ParameterBuilder.GetCapabilityWith(new[] { "p-224", "b-233" }, new[] { "sha2-512", "sha2-224" })
                        })
                    .Build()
            }
        };

        [Test]
        [TestCaseSource(nameof(parameters))]
        public void ShouldCreate1TestGroupForEachCombinationOfCurveHashAlg(int expectedGroups, Parameters parameters)
        {
            var curveFactoryMock = new Mock<IEccCurveFactory>();
            curveFactoryMock
                .Setup(s => s.GetCurve(It.IsAny<Curve>()))
                .Returns(new PrimeCurve(Curve.B163, 0, 0, new EccPoint(0, 0), 0));

            var eccDsaMock = new Mock<IDsaEcc>();
            eccDsaMock
                .Setup(s => s.GenerateKeyPair(It.IsAny<EccDomainParameters>()))
                .Returns(new EccKeyPairGenerateResult(new EccKeyPair(new EccPoint(1, 2), 3)));

            var eccDsaFactoryMock = new Mock<IDsaEccFactory>();
            eccDsaFactoryMock
                .Setup(s => s.GetInstance(It.IsAny<HashFunction>(), It.IsAny<EntropyProviderTypes>()))
                .Returns(eccDsaMock.Object);

            var subject = new TestGroupGenerator(eccDsaFactoryMock.Object, curveFactoryMock.Object);
            var result = subject.BuildTestGroups(parameters);
            Assert.AreEqual(expectedGroups, result.Count());
        }
    }
}
