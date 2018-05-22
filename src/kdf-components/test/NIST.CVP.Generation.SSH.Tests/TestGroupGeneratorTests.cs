using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.SSH.Tests
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
                    .WithCipher(new [] {"aes-128"})
                    .WithHashAlg(new[] {"sha2-512"})
                    .Build()
            },
            new object[]
            {
                3,
                new ParameterBuilder()
                    .WithCipher(new[] {"tdes"})
                    .WithHashAlg(new[] {"sha2-256", "sha2-384", "sha2-512"})
                    .Build()
            },
            new object[]
            {
                20,
                new ParameterBuilder()
                    .WithCipher(ParameterValidator.VALID_CIPHERS)
                    .WithHashAlg(ParameterValidator.VALID_HASH_ALGS)
                    .Build()
            }
        };
        [Test]
        [TestCaseSource(nameof(parameters))]
        public void ShouldCreateATestGroupForEachCombinationOfVersionAndHash(int expectedGroupsCreated, Parameters parameters)
        {
            var subject = new TestGroupGenerator();
            var results = subject.BuildTestGroups(parameters);
            Assert.AreEqual(expectedGroupsCreated, results.Count());
        }
    }
}
