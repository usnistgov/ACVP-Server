using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_CTR.Tests
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorKnownAnswerTestTests
    {
        // keyingOption does not matter
        private static object[] parameters = new object[]
        {
            new object[]
            {
                "No keyingOption, one direction",
                5, // 4 * 0 * 1
                new ParameterBuilder()
                    .WithKeyingOption(new int[] { }) // 0
                    .WithDirection(new[] {"encrypt"})  // 1
                    .Build()
            },
            new object[]
            {
                "One keyingOption, one direction",
                5, // 4 * 1 * 1
                new ParameterBuilder()
                    .WithKeyingOption(new[] {1}) // 1
                    .WithDirection(new[] {"encrypt"})  // 1
                    .Build()
            },
            new object[]
            {
                "One keyingOption, two directions",
                10, // 4 * 2 * 2
                new ParameterBuilder()
                    .WithKeyingOption(new[] {1}) // 2
                    .WithDirection(new[] {"encrypt", "decrypt"}) // 2
                    .Build()
            },
            new object[]
            {
                "Two keyingOptions, two directions",
                10, // 4 * 3 * 2
                new ParameterBuilder()
                    .WithKeyingOption(new[] {1, 2}) // 3
                    .WithDirection(new[] {"encrypt", "decrypt"}) // 2
                    .Build()
            }
        };
        [Test]
        [TestCaseSource(nameof(parameters))]
        public void ShouldCreate5TestGroupsForEachCombinationOfKeyingOptionAndDirection(string label, int expectedGroupsCreated, Parameters parameters)
        {
            var subject = new TestGroupGeneratorKnownAnswerTest();

            var results = subject.BuildTestGroups(parameters);
            Assert.AreEqual(expectedGroupsCreated, results.Count());
            Assert.IsTrue(results.Select(tg => (TestGroup)tg).All(tg => tg.StaticGroupOfTests));
        }
    }
}
