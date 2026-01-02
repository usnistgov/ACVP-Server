using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.TLSv13.RFC8446;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.TLS_v13
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorTests
    {
        private readonly TestGroupGenerator _subject = new TestGroupGenerator(new Random800_90());

        private static object[] _testParameters = new object[]
        {
            new object[]
            {
                5, // not sure if this is the intended value
                new ParameterBuilder()
                    .WithHashAlgs(new[] {HashFunctions.None})
                    .Build()
            },
            new object[]
            {
                5,
                new ParameterBuilder()
                    .WithHashAlgs(new[] {HashFunctions.Sha1})
                    .Build()
            },
            new object[]
            {
                10, // not sure if this is the intended value
                new ParameterBuilder()
                    .WithHashAlgs(new[] {HashFunctions.Sha1, HashFunctions.Sha3_d512})
                    .Build()
            },
        };

        [Test]
        [TestCaseSource(nameof(_testParameters))]
        public async Task ShouldCreateProperNumberOfGroups(int expectedNumberOfGroups, Parameters parameters)
        {
            var result = await _subject.BuildTestGroupsAsync(parameters);

            Assert.That(result.Count, Is.EqualTo(expectedNumberOfGroups));
        }
    }
}
