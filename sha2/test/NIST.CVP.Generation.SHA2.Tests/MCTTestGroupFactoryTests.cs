using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NIST.CVP.Generation.SHA2.Tests
{
    [TestFixture]
    public class MCTTestGroupFactoryTests
    {
        private static object[] parameters = new object[]
        {
            new object[]
            {
                0,
                new ParameterValidatorTests.ParameterBuilder()
                    .WithMode(new string[] { })
                    .WithDigestSize(new string[] { })
                    .Build()
            },
            new object[]
            {
                1,
                new ParameterValidatorTests.ParameterBuilder()
                    .WithMode(new string[] { "sha1" })
                    .WithDigestSize(new string[] { "160" })
                    .Build()
            },
            new object[]
            {
                3,
                new ParameterValidatorTests.ParameterBuilder()
                    .WithMode(new string[] { "sha1", "sha2", "sha2"})
                    .WithDigestSize(new string[] { "160", "512", "224"})
                    .Build()
            },
            new object[]
            {
                7,
                new ParameterValidatorTests.ParameterBuilder()
                    .WithMode(new string[] { "sha1", "sha2", "sha2", "sha2", "sha2", "sha2", "sha2"})
                    .WithDigestSize(new string[] { "160", "224", "256", "384", "512", "512/224", "512/256"})
                    .Build()
            },
        };

        [Test]
        [TestCaseSource(nameof(parameters))]
        public void ShouldCreate1TestGroupForEachDigestSize(int expectedGroupsCreated, Parameters parameters)
        {
            var subject = new MCTTestGroupFactory();

            var results = subject.BuildMCTTestGroups(parameters);
            Assert.AreEqual(expectedGroupsCreated, results.Count());
        }
    }
}
