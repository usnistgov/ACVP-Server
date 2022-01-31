using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.KAS.v1_0.ECC_Component;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.KAS.EccComponent
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
        public async Task ShouldReturnOneGroupPerValidCurve(string label, string[] curves, int expectedCount)
        {
            var p = new ParameterBuilder().WithCurves(curves).Build();

            var result = await _subject.BuildTestGroupsAsync(p);

            Assert.AreEqual(expectedCount, result.Count());
        }
    }
}
