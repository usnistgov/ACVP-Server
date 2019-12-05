using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using NIST.CVP.Generation.KDF_Components.v1_0.TLS;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.TLS.Tests
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
                    .WithVersion(new [] {"v1.0/1.1"})
                    .Build()
            },
            new object[]
            {
                3,
                new ParameterBuilder()
                    .WithVersion(new[] {"v1.2"})
                    .WithHashAlg(new[] {"sha2-256", "sha2-384", "sha2-512"})
                    .Build()
            },
            new object[]
            {
                4,
                new ParameterBuilder()
                    .WithVersion(new[] {"v1.0/1.1", "v1.2"})
                    .WithHashAlg(new[] {"sha2-256", "sha2-384", "sha2-512"})
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
