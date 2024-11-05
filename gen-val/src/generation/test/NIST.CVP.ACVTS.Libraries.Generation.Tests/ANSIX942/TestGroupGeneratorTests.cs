using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.AnsiX942.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.ANSIX942;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.ANSIX942
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorTests
    {
        private static object[] _parameters =
        {
            new object[]
            {
                1,
                new ParameterBuilder()
                    .WithKdfMode(new [] {AnsiX942Types.Der})
                    .WithOid(new [] {AnsiX942Oids.TDES})
                    .WithKeyLength(new MathDomain().AddSegment(new ValueDomainSegment(256)))
                    .WithOtherInfoLength(new MathDomain().AddSegment(new ValueDomainSegment(256)))
                    .WithZzLen(new MathDomain().AddSegment(new ValueDomainSegment(256)))
                    .WithHashAlg(new [] {"sha2-224"})
                    .Build()
            },
            new object[]
            {
                (1 * 2) + (2 * 2),  // concat + der
                new ParameterBuilder()
                    .WithKdfMode(new [] {AnsiX942Types.Concat, AnsiX942Types.Der})
                    .WithOid(new [] {AnsiX942Oids.TDES, AnsiX942Oids.AES_128_KW})
                    .WithKeyLength(new MathDomain().AddSegment(new ValueDomainSegment(256)))
                    .WithOtherInfoLength(new MathDomain().AddSegment(new ValueDomainSegment(256)))
                    .WithZzLen(new MathDomain().AddSegment(new ValueDomainSegment(256)))
                    .WithHashAlg(new [] {"sha2-224", "sha3-256"})
                    .Build()
            },
            new object[]
            {
                (1 * 11) + (4 * 11),    // (concat * hash) + (der * oid * hash)
                new ParameterBuilder()
                    .WithKdfMode(EnumHelpers.GetEnumsWithoutDefault<AnsiX942Types>().ToArray())
                    .WithOid(EnumHelpers.GetEnumsWithoutDefault<AnsiX942Oids>().ToArray())
                    .WithKeyLength(new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), ParameterValidator.MIN_KEY_LEN, ParameterValidator.MAX_KEY_LEN)))
                    .WithOtherInfoLength(new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), ParameterValidator.MIN_OTHER_INFO_LEN, ParameterValidator.MAX_OTHER_INFO_LEN)))
                    .WithZzLen(new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), ParameterValidator.MIN_ZZ_LEN, ParameterValidator.MAX_ZZ_LEN)))
                    .WithHashAlg(ParameterValidator.VALID_HASH_ALG)
                    .Build()
            }
        };
        [Test]
        [TestCaseSource(nameof(_parameters))]
        public async Task ShouldCreateATestGroupForEachCombination(int expectedGroupsCreated, Parameters parameters)
        {
            var subject = new TestGroupGenerator();
            var results = await subject.BuildTestGroupsAsync(parameters);
            Assert.That(results.Count(), Is.EqualTo(expectedGroupsCreated));
        }
    }
}
