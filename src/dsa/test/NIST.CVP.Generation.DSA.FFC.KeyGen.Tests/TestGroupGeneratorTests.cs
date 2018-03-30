using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.FFC.KeyGen.Tests
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorGTests
    {
        private static object[] parameters =
        {
            new object[]
            {
                1,
                new ParameterBuilder()
                    .WithCapabilities(new []
                        {
                            ParameterBuilder.GetCapabilityWith(2048, 224)
                        })
                    .Build()
            },
            new object[]
            {
                2,
                new ParameterBuilder()
                    .WithCapabilities(new []
                        {
                            ParameterBuilder.GetCapabilityWith(2048, 224),
                            ParameterBuilder.GetCapabilityWith(3072, 256)
                        })
                    .Build()
            },
            new object[]
            {
                3,
                new ParameterBuilder()
                    .WithCapabilities(new []
                        {
                            ParameterBuilder.GetCapabilityWith(2048, 224),
                            ParameterBuilder.GetCapabilityWith(2048, 256),
                            ParameterBuilder.GetCapabilityWith(3072, 256)
                        })
                    .Build()
            }
        };

        [Test]
        [TestCaseSource(nameof(parameters))]
        public void ShouldCreate1TestGroupForEachCombinationOfModeLN(int expectedGroups, Parameters parameters)
        {
            var dsaMock = new Mock<IDsaFfc>();
            dsaMock
                .Setup(s => s.GenerateDomainParameters(It.IsAny<FfcDomainParametersGenerateRequest>()))
                .Returns(new FfcDomainParametersGenerateResult(new FfcDomainParameters(1, 2, 3), new DomainSeed(4), new Counter(5)));

            var subject = new TestGroupGenerator(dsaMock.Object);
            var result = subject.BuildTestGroups(parameters);
            Assert.AreEqual(expectedGroups, result.Count());
        }
    }
}
