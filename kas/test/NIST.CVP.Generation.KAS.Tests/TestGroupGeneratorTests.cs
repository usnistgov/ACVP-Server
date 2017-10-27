using System;
using System.Linq;
using Moq;
using NIST.CVP.Crypto.DSA;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.KAS.FFC;
using NIST.CVP.Generation.KAS.Tests.Builders;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KAS.Tests
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorTests
    {
        private TestGroupGenerator _subject;
        private Mock<IDsaFfcFactory> _dsaFactory;
        private Mock<IDsaFfc> _dsa;
        private Mock<IShaFactory> _shaFactory;
        private Mock<ISha> _sha;

        [SetUp]
        public void Setup()
        {
            _dsaFactory = new Mock<IDsaFfcFactory>();
            _dsa = new Mock<IDsaFfc>();
            _shaFactory = new Mock<IShaFactory>();
            _sha = new Mock<ISha>();

            _dsaFactory
                .Setup(s => s.GetInstance(It.IsAny<HashFunction>(), EntropyProviderTypes.Random))
                .Returns(_dsa.Object);

            _dsa.SetupGet(s => s.Sha).Returns(_sha.Object);
            _dsa.Setup(s => s.GenerateDomainParameters(It.IsAny<FfcDomainParametersGenerateRequest>()))
                .Returns(
                    new FfcDomainParametersGenerateResult(
                        new FfcDomainParameters(0, 0, 0), 
                        new DomainSeed(0),
                        new Counter(0)
                    )
                );

            _shaFactory.Setup(s => s.GetShaInstance(It.IsAny<HashFunction>())).Returns(_sha.Object);

            _sha.SetupGet(s => s.HashFunction).Returns(new HashFunction(ModeValues.SHA2, DigestSizes.d256));
            _sha.Setup(s => s.HashMessage(It.IsAny<BitString>())).Returns(new HashResult(new BitString(0)));

            _subject = new TestGroupGenerator(_dsaFactory.Object, _shaFactory.Object);
        }

        private static object[] _testShouldReturnCorrectNumberOfGroups = new object[]
        {
            new object[]
            {
                new ParameterBuilder()
                    .WithSchemes(
                        new SchemesBuilder()
                            .WithDhEphem(
                                new SchemeBuilder()
                                    .WithRole(new string[] { "initiator" }) // 1
                                    .WithNoKdfNoKc(
                                        new NoKdfNoKcBuilder()
                                            .WithParameterSets(
                                                new ParameterSetBuilder(false)
                                                    .WithFb(
                                                        new ParameterSetBaseBuilder(false)
                                                            .WithHashAlg(new string[] {"SHA2-256"}) // 1
                                                            .BuildParameterSetBaseFb()
                                                    )
                                                    .WithFc(null)
                                                    .BuildParameterSets()
                                            )
                                            .BuildNoKdfNoKc()
                                    )
                                    .WithKdfNoKc(null)
                                    .BuildDhEphem()
                            )
                            .BuildSchemes()
                    )
                    .BuildParameters(),
                2 // 1 * 2 (for test types)
            },
            new object[]
            {
                new ParameterBuilder()
                    .WithSchemes(
                        new SchemesBuilder()
                            .WithDhEphem(
                                new SchemeBuilder()
                                    .WithRole(new string[] { "initiator" }) // 1
                                    .WithNoKdfNoKc(
                                        new NoKdfNoKcBuilder()
                                            .WithParameterSets(
                                                new ParameterSetBuilder(false)
                                                    .WithFb(
                                                        new ParameterSetBaseBuilder(false)
                                                            .WithHashAlg(new string[] {"SHA2-256"}) // 1
                                                            .BuildParameterSetBaseFb()
                                                    )
                                                    .WithFc(new ParameterSetBaseBuilder(false)
                                                        .WithHashAlg(new string[] {"SHA2-256"}) // 2
                                                        .BuildParameterSetBaseFc())
                                                    .BuildParameterSets()
                                            )
                                            .BuildNoKdfNoKc()
                                    )
                                    .WithKdfNoKc(null)
                                    .BuildDhEphem()
                            )
                            .BuildSchemes()
                    )
                    .BuildParameters(),
                4 // 2 * 2 (for test types)
            },
            new object[]
            {
                new ParameterBuilder()
                    .WithSchemes(
                        new SchemesBuilder()
                            .WithDhEphem(
                                new SchemeBuilder()
                                    .WithRole(new string[] { "responder" }) // 1
                                    .WithNoKdfNoKc(
                                        new NoKdfNoKcBuilder()
                                            .WithParameterSets(
                                                new ParameterSetBuilder(false)
                                                    .WithFb(
                                                        new ParameterSetBaseBuilder(false)
                                                            .WithHashAlg(new string[] {"SHA2-256"}) // 1
                                                            .BuildParameterSetBaseFb()
                                                    )
                                                    .WithFc(new ParameterSetBaseBuilder(false)
                                                        .WithHashAlg(new string[] {"SHA2-256", "SHA2-512"}) // 2 - only use one sha per parameter set for NoKdfNoKc
                                                        .BuildParameterSetBaseFc())
                                                    .BuildParameterSets()
                                            )
                                            .BuildNoKdfNoKc()
                                    )
                                    .WithKdfNoKc(null)
                                    .BuildDhEphem()
                            )
                            .BuildSchemes()
                    )
                    .BuildParameters(),
                4 // 2 * 2 (for test types)
            },
            new object[]
            {
                new ParameterBuilder()
                    .WithSchemes(
                        new SchemesBuilder()
                            .WithDhEphem(
                                new SchemeBuilder()
                                    .WithRole(new string[] { "initiator", "responder" }) // 1,2,3,4
                                    .WithNoKdfNoKc(
                                        new NoKdfNoKcBuilder()
                                            .WithParameterSets(
                                                new ParameterSetBuilder(false)
                                                    .WithFb(
                                                        new ParameterSetBaseBuilder(false)
                                                            .WithHashAlg(new string[] {"SHA2-256"}) // 1, 2
                                                            .BuildParameterSetBaseFb()
                                                    )
                                                    .WithFc(new ParameterSetBaseBuilder(false)
                                                        .WithHashAlg(new string[] {"SHA2-256"}) // 3, 4
                                                        .BuildParameterSetBaseFc())
                                                    .BuildParameterSets()
                                            )
                                            .BuildNoKdfNoKc()
                                    )
                                    .WithKdfNoKc(null)
                                    .BuildDhEphem()
                            )
                            .BuildSchemes()
                    )
                    .BuildParameters(),
                8 // 4 * 2 (for test types)
            },
            new object[]
            {
                new ParameterBuilder()
                    .WithSchemes(
                        new SchemesBuilder()
                            .WithDhEphem(
                                new SchemeBuilder()
                                    .WithRole(new string[] { "initiator", "responder" }) // 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12
                                    .WithNoKdfNoKc(
                                        new NoKdfNoKcBuilder()
                                            .WithParameterSets(
                                                new ParameterSetBuilder(false)
                                                    .WithFb(
                                                        new ParameterSetBaseBuilder(false)
                                                            .WithHashAlg(new string[] {"SHA2-256"}) // 1, 2
                                                            .BuildParameterSetBaseFb()
                                                    )
                                                    .WithFc(new ParameterSetBaseBuilder(false)
                                                        .WithHashAlg(new string[] {"SHA2-256"}) // 3, 4
                                                        .BuildParameterSetBaseFc())
                                                    .BuildParameterSets()
                                            )
                                            .BuildNoKdfNoKc()
                                    )
                                    .WithKdfNoKc(
                                        new KdfNoKcBuilder()
                                            .WithKdfOptions(
                                                new KdfOptionsBuilder()
                                                    .WithAsn1("uPartyInfo||vPartyInfo")  // 5, 6, 7, 8
                                                    .WithConcatenation("uPartyInfo||vPartyInfo") // 9, 10, 11, 12
                                                    .BuildKdfOptions()
                                            )
                                            .WithParameterSets(
                                                new ParameterSetBuilder(true)
                                                    .WithFb(
                                                        new ParameterSetBaseBuilder(true)
                                                            .WithHashAlg(new string[] {"SHA2-256"}) // 5, 6, 9, 10
                                                            .WithMacOptions(
                                                                new MacOptionsBuilder()
                                                                    .WithHmac2_224(
                                                                        new MacOptionsBaseBuilder(true)
                                                                            .BuildHmac2_224() // 5, 6, 9, 10
                                                                    )
                                                                    .WithAesCcm(null)
                                                                    .WithCmac(null)
                                                                    .WithHmac2_256(null)
                                                                    .WithHmac2_384(null)
                                                                    .WithHmac2_512(null)
                                                                    .BuildMacOptions()
                                                            )
                                                            .BuildParameterSetBaseFb()
                                                    )
                                                    .WithFc(new ParameterSetBaseBuilder(true)
                                                        .WithHashAlg(new string[] {"SHA2-256"}) // 7, 8, 11, 12
                                                        .WithMacOptions(
                                                            new MacOptionsBuilder()
                                                                .WithHmac2_224(
                                                                    new MacOptionsBaseBuilder(true)
                                                                        .BuildHmac2_224() // 7, 8, 11, 12
                                                                )
                                                                .WithAesCcm(null)
                                                                .WithCmac(null)
                                                                .WithHmac2_256(null)
                                                                .WithHmac2_384(null)
                                                                .WithHmac2_512(null)
                                                                .BuildMacOptions()
                                                        )
                                                        .BuildParameterSetBaseFc())
                                                    .BuildParameterSets()
                                            )
                                            .BuildKdfNoKc()
                                    )
                                    .BuildDhEphem()
                            )
                            .BuildSchemes()
                    )
                    .BuildParameters(),
                24 // 12 * 2 (for test types)
            },
        };

        [Test]
        [TestCaseSource(nameof(_testShouldReturnCorrectNumberOfGroups))]
        public void ShouldReturnCorrectNumberOfGroups(Parameters parm, int expectedNumberOfGroups)
        {
            var result = _subject.BuildTestGroups(parm);

            Assert.AreEqual(expectedNumberOfGroups, result.Count());
        }
    }
}