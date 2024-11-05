using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Generation.AES_FFX.v1_0.Base;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.AES.FF
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorTests
    {
        private TestGroupGenerator _subject;

        private static IEnumerable<object> _testData = new List<object>()
        {
            new object[]
            {
                // label
                "test1 - 0",
                // algo
                AlgoMode.AES_FF1_v1_0,
                // direction
                new string[] { },
                // key len
                new int[] { },
                // tweak
                null,
                // capabilities
                null,
                // expected
                0
            },
            new object[]
            {
                // label
                "test2 - 0",
                // algo
                AlgoMode.AES_FF1_v1_0,
                // direction
                new string[] { },
                // key len
                new int[] { },
                // tweak
                new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 0, 128, 8)), 
                // capabilities
                new List<Capability>
                {
                    new Capability()
                    {
                        Alphabet = "0123456789",
                        Radix = 10,
                        MinLen = 20,
                        MaxLen = 26
                    }
                }.ToArray(),
                // expected
                0
            },
            new object[]
            {
                // label
                "test3 - 0",
                // algo
                AlgoMode.AES_FF1_v1_0,
                // direction
                new string[] { },
                // key len
                new int[] { 128 },
                // tweak
                new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 0, 128, 8)), 
                // capabilities
                new List<Capability>
                {
                    new Capability()
                    {
                        Alphabet = "0123456789",
                        Radix = 10,
                        MinLen = 20,
                        MaxLen = 26
                    }
                }.ToArray(),
                // expected
                0
            },
            new object[]
            {
                // label
                "test4 - 1",
                // algo
                AlgoMode.AES_FF1_v1_0,
                // direction
                new string[] { "encrypt" },
                // key len
                new int[] { 128 },
                // tweak
                new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 0, 128, 8)), 
                // capabilities
                new List<Capability>
                    {
                        new Capability()
                        {
                            Alphabet = "0123456789",
                            Radix = 10,
                            MinLen = 20,
                            MaxLen = 26
                        }
                    }.ToArray(),
                // expected
                1
            },
            new object[]
            {
                // label
                "test5 - 6",
                // algo
                AlgoMode.AES_FF1_v1_0,
                // direction
                new string[] { "encrypt", "decrypt" },
                // key len
                new int[] { 128, 192, 256 },
                // tweak
                new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 0, 128, 8)), 
                // capabilities
                new List<Capability>
                {
                    new Capability()
                    {
                        Alphabet = "0123456789",
                        Radix = 10,
                        MinLen = 20,
                        MaxLen = 26
                    }
                }.ToArray(),
                // expected
                6
            },
            new object[]
            {
                // label
                "test6 - 6",
                // algo
                AlgoMode.AES_FF1_v1_0,
                // direction
                new string[] { "encrypt", "decrypt" },
                // key len
                new int[] { 128, 192, 256 },
                // tweak
                new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 0, 128, 8)), 
                // capabilities
                new List<Capability>
                {
                    new Capability()
                    {
                        Alphabet = "0123456789",
                        Radix = 10,
                        MinLen = 20,
                        MaxLen = 26
                    },
                    new Capability()
                    {
                        Alphabet = "abcde",
                        Radix = 5,
                        MinLen = 20,
                        MaxLen = 26
                    },
                }.ToArray(),
                // expected
                12
            },
        };

        [Test]
        [TestCaseSource(nameof(_testData))]
        public async Task ShouldReturnCorrectNumberOfTestGroups(
            string label,
            AlgoMode algoMode,
            string[] direction,
            int[] keyLen,
            MathDomain tweak,
            Capability[] capabilities,
            int expectedResultCount
        )
        {
            string algo = algoMode == AlgoMode.AES_FF1_v1_0 ? "ACVP-AES-FF1" : "ACVP-AES-FF3-1";

            Parameters p = new ParameterBuilder()
                .WithAlgorithm(algo)
                .WithKeyLen(keyLen)
                .WithDirection(direction)
                .WithTweakLen(tweak)
                .WithCapabilities(capabilities)
                .Build();

            _subject = new TestGroupGenerator();
            var result = await _subject.BuildTestGroupsAsync(p);

            Assert.That(result.Count, Is.EqualTo(expectedResultCount));
        }
    }
}
