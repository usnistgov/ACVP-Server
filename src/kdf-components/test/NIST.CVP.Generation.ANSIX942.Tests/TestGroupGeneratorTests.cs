using System;
using System.Linq;
using NIST.CVP.Generation.KDF_Components.v1_0.ANSIX942;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.ANSIX942.Tests
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
                    .WithKdfMode(new [] {"DER"})
                    .WithKeyLength(new MathDomain().AddSegment(new ValueDomainSegment(256)))
                    .WithOtherInfoLength(new MathDomain().AddSegment(new ValueDomainSegment(256)))
                    .WithZzLen(new MathDomain().AddSegment(new ValueDomainSegment(256)))
                    .WithHashAlg(new [] {"sha2-224"})
                    .Build()
            },
            new object[]
            {
                4,
                new ParameterBuilder()
                    .WithKdfMode(new [] {"DER", "concatenation"})
                    .WithKeyLength(new MathDomain().AddSegment(new ValueDomainSegment(256)))
                    .WithOtherInfoLength(new MathDomain().AddSegment(new ValueDomainSegment(256)))
                    .WithZzLen(new MathDomain().AddSegment(new ValueDomainSegment(256)))
                    .WithHashAlg(new [] {"sha2-224", "sha3-256"})
                    .Build()
            },
            new object[]
            {
                2 * 11 * 3,
                new ParameterBuilder()
                    .WithKdfMode(ParameterValidator.VALID_MODES)
                    .WithKeyLength(new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), ParameterValidator.MIN_KEY_LEN, ParameterValidator.MAX_KEY_LEN)))
                    .WithOtherInfoLength(new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), ParameterValidator.MIN_OTHER_INFO_LEN, ParameterValidator.MAX_OTHER_INFO_LEN)))
                    .WithZzLen(new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), ParameterValidator.MIN_ZZ_LEN, ParameterValidator.MAX_ZZ_LEN)))
                    .WithHashAlg(ParameterValidator.VALID_HASH_ALG)
                    .Build()
            }
        };
        [Test]
        [TestCaseSource(nameof(parameters))]
        public void ShouldCreateATestGroupForEachCombination(int expectedGroupsCreated, Parameters parameters)
        {
            var subject = new TestGroupGenerator();
            var results = subject.BuildTestGroups(parameters);
            Assert.AreEqual(expectedGroupsCreated, results.Count());
        }
    }
}
