using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_CTR.Tests
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorCounterTests
    {
        private static object[] parameters =
        {
            new object[]
            {
                0, // 0 * 1
                new ParameterBuilder()
                    .WithKeyingOption(new int[] { }) // 0
                    .WithDirection(new[] {"encrypt"})  // 1
                    .WithOverflowCounter(false)
                    .Build()
            },
            new object[]
            {
                1, // 1 * 1
                new ParameterBuilder()
                    .WithKeyingOption(new[] {1}) // 1
                    .WithDirection(new[] {"encrypt"})  // 1
                    .WithOverflowCounter(false)
                    .Build()
            },
            new object[]
            {
                3, // 3 * 1
                new ParameterBuilder()
                    .WithKeyingOption(new[] {1, 2}) // 2
                    .WithDirection(new[] {"encrypt", "decrypt"}) // 2
                    .WithOverflowCounter(false) // 1
                    .Build()
            },
            new object[]
            {
                6, // 3 * 2
                new ParameterBuilder()
                    .WithKeyingOption(new[] {1, 2}) // 3
                    .WithDirection(new[] {"encrypt", "decrypt"}) // 2
                    .WithOverflowCounter(true)
                    .Build()
            }
        };
        [Test]
        [TestCaseSource(nameof(parameters))]
        public void ShouldCreateATestGroupForEachCombinationOfKeyingOptionAndDirection(int expectedGroupsCreated, Parameters parameters)
        {
            var subject = new TestGroupGeneratorCounter();

            var results = subject.BuildTestGroups(parameters);
            Assert.AreEqual(expectedGroupsCreated, results.Count());
        }
    }
}
