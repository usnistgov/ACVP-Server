using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.SNMP.Tests
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
                    .WithEngineId(new [] {"abcdabcdabcdabcdabcd"})
                    .WithPasswordLength(new MathDomain().AddSegment(new ValueDomainSegment(128)))
                    .Build()
            },
            new object[]
            {
                2,
                new ParameterBuilder()
                    .WithEngineId(new [] {"abcdabcdabcdabcdabcd"})
                    .WithPasswordLength(new MathDomain().AddSegment(new RangeDomainSegment(null, 128, 1024)))
                    .Build()
            },
            new object[]
            {
                4,
                new ParameterBuilder()
                    .WithEngineId(new [] {"abcdabcdabcdabcdabcd", "abcdabcdabcdabcdef"})
                    .WithPasswordLength(new MathDomain().AddSegment(new ValueDomainSegment(128)).AddSegment(new ValueDomainSegment(512)))
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
