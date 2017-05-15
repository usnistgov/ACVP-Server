using System;
using System.Collections.Generic;
using Moq;
using NIST.CVP.Crypto.KeyWrap.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NUnit.Framework;

namespace NIST.CVP.Generation.KeyWrap.Tests
{
    [TestFixture]
    public class TestVectorFactoryTests
    {

        private TestVectorFactory _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new TestVectorFactory();
        }

        [Test]
        [TestCase("")]
        [TestCase("AES")]
        [TestCase("KW-AES")]
        public void ShouldThrowExceptionOnInvalidAlgorithm(string algorithm)
        {
            var p = new Parameters {Algorithm = algorithm};

            Assert.Throws(typeof(ArgumentException), () => _subject.BuildTestVectorSet(p));
        }

        #region GetParametersAndExpectedGroups
        private static List<object> GetParametersAndExpectedGroups()
        {
            Random800_90 randy = new Random800_90();

            List<object> list = new List<object>()
            {
                new object[]
                {
                    "Minimal Inputs",
                    "AES-KW", // 1
                    new int[] { 128 }, // 1
                    new string[] { "encrypt" }, // 1
                    new string[] { "cipher" }, // 1
                    new MathDomain().AddSegment(new ValueDomainSegment(128)), // 1
                    1 // ! * 1 * 1 * 1 * 1
                },
                new object[]
                {
                    "Additional key w/ Minimal Inputs",
                    "AES-KW", // 1
                    new int[] { 128, 192 }, // 2
                    new string[] { "encrypt" }, // 1
                    new string[] { "cipher" }, // 1
                    new MathDomain().AddSegment(new ValueDomainSegment(128)), // 1
                    2 // ! * 2 * 1 * 1 * 1
                },
                new object[]
                {
                    "All inputs at maximum, except PtLen",
                    "AES-KW", // 1
                    new int[] { 128, 192, 256 }, // 3
                    new string[] { "encrypt", "decrypt" }, // 2
                    new string[] { "cipher", "inverse" }, // 2
                    new MathDomain().AddSegment(new ValueDomainSegment(128)), // 1
                    12 // 1 * 3 * 2 * 2 * 1
                },
                new object[]
                {
                    "Testing math domain, 4 possible values in domain, 4 pulled",
                    "AES-KW", // 1
                    new int[] { 128 }, // 1
                    new string[] { "encrypt" }, // 1
                    new string[] { "cipher" }, // 1
                    new MathDomain()
                        .AddSegment(new RangeDomainSegment(randy, 128, 256, 128)) // 2 possible values
                        .AddSegment(new RangeDomainSegment(randy, 192, 320, 128)), // 2 possible values, 1 of which is the max value
                    // 4 values total from range
                    4 // 1 * 1 * 1 * 1 * 4
                },
                new object[]
                {
                    "Testing math domain, large range of values, no mod 64 that isn't mod 128, 3 pulled",
                    "AES-KW", // 1
                    new int[] { 128 }, // 1
                    new string[] { "encrypt" }, // 1
                    new string[] { "cipher" }, // 1
                    new MathDomain()
                        .AddSegment(new RangeDomainSegment(randy, 128, 128*200, 128)), // lots of possible values, no mod 64 that isn't mod 128
                    // 3 values total from range
                    3 // 1 * 1 * 1 * 1 * 3
                },
                new object[]
                {
                    "Testing math domain, large range of values, 5 pulled",
                    "AES-KW", // 1
                    new int[] { 128 }, // 1
                    new string[] { "encrypt" }, // 1
                    new string[] { "cipher" }, // 1
                    new MathDomain()
                        .AddSegment(new RangeDomainSegment(randy, 128, 128*200, 64)), // lots of possible values
                    // 5 values total from range
                    5 // 1 * 1 * 1 * 1 * 5
                },
                new object[]
                {
                    "Maximum number of groups for a test vector set",
                    "AES-KW", // 1
                    new int[] { 128, 192, 256 }, // 3
                    new string[] { "encrypt", "decrypt" }, // 2
                    new string[] { "cipher", "inverse" }, // 2
                    new MathDomain()
                        .AddSegment(new RangeDomainSegment(randy, 128, 128*200, 64)), // lots of possible values
                    // 5 values total from range
                    60 // 1 * 3 * 2 * 2 * 5
                }
            };

            return list;
        }
        #endregion GetParametersAndExpectedGroups

        [Test]
        [TestCaseSource(nameof(GetParametersAndExpectedGroups))]
        public void ShouldCreateCorrectNumberOfGroups(
            string testLabel, 
            string algorithm,
            int[] keyLen,
            string[] direction,
            string[] kwCipher,
            MathDomain ptLen,
            int expectedNumberOfGroups)
        {
            Parameters parameters = new Parameters()
            {
                Direction = direction,
                PtLen = ptLen,
                KwCipher = kwCipher,
                KeyLen = keyLen,
                Algorithm = algorithm
            };

            var result = _subject.BuildTestVectorSet(parameters);

            Assert.AreEqual(expectedNumberOfGroups, result.TestGroups.Count);
        }
    }
}
