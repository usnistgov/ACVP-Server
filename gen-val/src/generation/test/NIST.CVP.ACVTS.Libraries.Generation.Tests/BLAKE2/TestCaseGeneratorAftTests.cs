using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.Blake2;
using NIST.CVP.ACVTS.Libraries.Generation.BLAKE2.v1_0;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using Moq;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.BLAKE2
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorAftTests
    {
        [Test]
        public async Task ShouldGenerateBlake2bAftTestCaseWithDigest()
        {
            var group = new TestGroup
            {
                TestType = "AFT",
                DigestLength = new MathDomain().AddSegment(new ValueDomainSegment(512)),
                MessageLength = new MathDomain().AddSegment(new RangeDomainSegment(null, 0, 1024, 8)),
                KeyLength = new MathDomain().AddSegment(new RangeDomainSegment(null, 0, 512, 8))
            };
            var oracle = new Mock<IOracle>();
            oracle
                .Setup(s => s.GetBlake2CaseAsync(It.IsAny<Blake2Parameters>()))
                .ReturnsAsync((Blake2Parameters p) => new Blake2Result
                {
                    Message = new BitString(p.MessageLength),
                    Key = p.KeyLength == 0 ? null : new BitString(p.KeyLength),
                    Digest = new BitString(p.HashFunction.DigestLength)
                });
            var subject = new TestCaseGeneratorAft(oracle.Object);

            subject.PrepareGenerator(group, false);
            var result = await subject.GenerateAsync(group, false);

            Assert.That(result.Success, Is.True, result.ErrorMessage);
            Assert.That(result.TestCase.Message.BitLength % 8, Is.EqualTo(0));
            Assert.That(result.TestCase.Key?.BitLength % 8 ?? 0, Is.EqualTo(0));
            Assert.That(result.TestCase.DigestLength, Is.EqualTo(512));
            Assert.That(result.TestCase.Digest.BitLength, Is.EqualTo(512));
            oracle.Verify(v => v.GetBlake2CaseAsync(It.Is<Blake2Parameters>(p =>
                p.HashFunction.Variant == Blake2Variant.Blake2b &&
                p.HashFunction.DigestLength == 512 &&
                p.MessageLength % 8 == 0 &&
                p.KeyLength % 8 == 0
            )), Times.Once);
        }

        [Test]
        public async Task ShouldKeepFixedCaseCountAndCoverBlockBoundaryPatterns()
        {
            var observedMessageLengths = new List<int>();
            var group = new TestGroup
            {
                TestType = "AFT",
                DigestLength = new MathDomain().AddSegment(new ValueDomainSegment(512)),
                MessageLength = new MathDomain().AddSegment(new RangeDomainSegment(null, 2000, 4000, 8)),
                KeyLength = new MathDomain().AddSegment(new ValueDomainSegment(0))
            };
            var oracle = new Mock<IOracle>();
            oracle
                .Setup(s => s.GetBlake2CaseAsync(It.IsAny<Blake2Parameters>()))
                .ReturnsAsync((Blake2Parameters p) =>
                {
                    observedMessageLengths.Add(p.MessageLength);
                    return new Blake2Result
                    {
                        Message = new BitString(p.MessageLength),
                        Digest = new BitString(p.HashFunction.DigestLength)
                    };
                });
            var subject = new TestCaseGeneratorAft(oracle.Object);

            subject.PrepareGenerator(group, false);
            for (var i = 0; i < subject.NumberOfTestCasesToGenerate; i++)
            {
                var result = await subject.GenerateAsync(group, false);
                Assert.That(result.Success, Is.True, result.ErrorMessage);
            }

            Assert.Multiple(() =>
            {
                Assert.That(subject.NumberOfTestCasesToGenerate, Is.EqualTo(25));
                Assert.That(observedMessageLengths, Has.Count.EqualTo(25));
                Assert.That(observedMessageLengths.Any(x => x % 1024 == 1016), Is.True);
                Assert.That(observedMessageLengths.Any(x => x % 1024 == 0), Is.True);
                Assert.That(observedMessageLengths.Any(x => x % 1024 == 8), Is.True);
            });
        }
    }
}
