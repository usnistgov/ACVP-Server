using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NIST.CVP.Generation.KDF_Components.v1_0.ANXIX963;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.ANSIX963.Tests
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorTests
    {
        private static object[] parameters = 
        {
            new object[]
            {
                0,  // Hash algo too small to make any valid groups
                new ParameterBuilder()
                    .WithFieldSize(new [] {256})
                    .WithKeyDataLength(new MathDomain().AddSegment(new ValueDomainSegment(256)))
                    .WithSharedInfoLength(new MathDomain().AddSegment(new ValueDomainSegment(256)))
                    .WithHashAlg(new [] {"sha2-224"})
                    .Build()
            },
            new object[]
            {
                1,
                new ParameterBuilder()
                    .WithFieldSize(new [] {256})
                    .WithKeyDataLength(new MathDomain().AddSegment(new ValueDomainSegment(256)))
                    .WithSharedInfoLength(new MathDomain().AddSegment(new ValueDomainSegment(256)))
                    .WithHashAlg(new [] {"sha2-256"})
                    .Build()
            },
            new object[]
            {
                3,
                new ParameterBuilder()
                    .WithFieldSize(new [] {256, 571})
                    .WithKeyDataLength(new MathDomain().AddSegment(new ValueDomainSegment(256)))
                    .WithSharedInfoLength(new MathDomain().AddSegment(new ValueDomainSegment(256)))
                    .WithHashAlg(new [] {"sha2-256", "sha2-512"})
                    .Build()
            },
            new object[]
            {
                12,
                new ParameterBuilder()
                    .WithFieldSize(new [] {283, 521})
                    .WithKeyDataLength(new MathDomain().AddSegment(new ValueDomainSegment(ParameterValidator.KEY_LENGTH_MINIMUM)).AddSegment(new ValueDomainSegment(ParameterValidator.KEY_LENGTH_MAXIMUM)))
                    .WithSharedInfoLength(new MathDomain().AddSegment(new ValueDomainSegment(ParameterValidator.SHARED_INFO_MINIMUM)).AddSegment(new ValueDomainSegment(ParameterValidator.SHARED_INFO_MAXIMUM)))
                    .WithHashAlg(ParameterValidator.VALID_HASH_ALGS)
                    .Build()
            }
        };
        [Test]
        [TestCaseSource(nameof(parameters))]
        public async Task ShouldCreateATestGroupForEachCombination(int expectedGroupsCreated, Parameters parameters)
        {
            var subject = new TestGroupGenerator();
            var results = await subject.BuildTestGroupsAsync(parameters);
            Assert.AreEqual(expectedGroupsCreated, results.Count());
        }
    }
}
