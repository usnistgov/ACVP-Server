using System.Linq;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.DSA.ECC.Enums;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KAS.EccComponent.Tests
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorTests
    {
        private TestGroupGenerator _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new TestGroupGenerator();
        }

        private static object[] _testCurves = new object[]
        {
            new object[]
            {
                "all curves",
                EnumHelpers.GetEnumDescriptions<Curve>().ToArray(),
                EnumHelpers.GetEnumDescriptions<Curve>().Count
            },
            new object[]
            {
                "one curve",
                new string[] { "p-192" },
                1
            },
            new object[]
            {
                "two curves",
                new string[] { "p-192", "p-521" },
                2
            },
        };

        [Test]
        [TestCaseSource(nameof(_testCurves))]
        public void ShouldReturnOneGroupPerValidCurve(string label, string[] curves, int expectedCount)
        {
            var p = new ParameterBuilder().WithCurves(curves).Build();

            var result = _subject.BuildTestGroups(p);

            Assert.AreEqual(expectedCount, result.Count());
        }
    }
}