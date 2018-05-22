using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_CCM.Tests
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

        #region Parameter test scenarios
        private static object[] GetTestParameterData()
        {

            List<object[]> list = new List<object[]>()
            {
                new object[]
                {
                    "minimum inputs",
                    new[] {128},
                    new MathDomain().AddSegment(new ValueDomainSegment(0)),
                    new MathDomain().AddSegment(new ValueDomainSegment(0)),
                    new MathDomain().AddSegment(new ValueDomainSegment(ParameterValidator.VALID_NONCE_LENGTHS.First())),
                    new MathDomain().AddSegment(new ValueDomainSegment(ParameterValidator.VALID_TAG_LENGTHS.First())),
                },
                new object[]
                {
                    "multiple inputs, single array",
                    new[] {128, 192, 256},
                    new MathDomain().AddSegment(new ValueDomainSegment(0)),
                    new MathDomain().AddSegment(new ValueDomainSegment(0)),
                    new MathDomain().AddSegment(new ValueDomainSegment(ParameterValidator.VALID_NONCE_LENGTHS.First())),
                    new MathDomain().AddSegment(new ValueDomainSegment(ParameterValidator.VALID_TAG_LENGTHS.First())),
                },
                new object[]
                {
                    "multiple differing inputs, in min/max array",
                    new[] {128},
                    new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 0, 32 * 8, 8)),
                    new MathDomain().AddSegment(new ValueDomainSegment(0)),
                    new MathDomain().AddSegment(new ValueDomainSegment(ParameterValidator.VALID_NONCE_LENGTHS.First())),
                    new MathDomain().AddSegment(new ValueDomainSegment(ParameterValidator.VALID_TAG_LENGTHS.First())),
                },
                new object[]
                {
                    "max number of groups, no 2^16",
                    new[] {128, 192, 256},
                    new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 0, 32 * 8, 8)),
                    new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 0, 32 * 8, 8)),
                    new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(),
                        ParameterValidator.VALID_NONCE_LENGTHS.First(), ParameterValidator.VALID_NONCE_LENGTHS.Last(), 8)),
                    new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(),
                        ParameterValidator.VALID_TAG_LENGTHS.First(), ParameterValidator.VALID_TAG_LENGTHS.Last(), 16)),
                },
                new object[]
                {
                    "max number of groups, max aad",
                    new[] {128, 192, 256},
                    new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 0, (1 << 19), 8)),
                    new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 0, 32 * 8, 8)),
                    new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(),
                        ParameterValidator.VALID_NONCE_LENGTHS.First(), ParameterValidator.VALID_NONCE_LENGTHS.Last(), 8)),
                    new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(),
                        ParameterValidator.VALID_TAG_LENGTHS.First(), ParameterValidator.VALID_TAG_LENGTHS.Last(), 16)),
                }
            };

            return list.ToArray();
        }
        #endregion Parameter test scenarios

        [Test]
        public void ShouldContainGroupsForEachTestType()
        {
            Parameters p = new ParameterBuilder().Build();

            var result = _subject.BuildTestGroups(p);

            foreach (TestTypes testType in Enum.GetValues(typeof(TestTypes)))
            {
                Assert.IsTrue(result.Any(a => a.TestType == testType.ToString()));
            }
        }

        /// <summary>
        /// DecryptionVerification creates test cases based on:
        /// 
        ///     - Each key size
        ///     - The minimum and maximum AAD length (not including 2^16 )       
        ///     - The minimum and maximum Payload length
        ///     - Each nonce length
        ///     - Each Tag length
        /// </summary>
        [Test]
        [TestCaseSource(nameof(GetTestParameterData))]
        public void ShouldHaveValidNumberOfGroupsForDecryptionVerification(
            string testLabel,
            int[] keyLen,
            MathDomain aadLen,
            MathDomain ptLen,
            MathDomain ivLen,
            MathDomain tagLen
        )
        {
            TestTypes testType = TestTypes.DecryptionVerification;

            Parameters p = new Parameters()
            {
                Algorithm = "AES CCM",
                KeyLen = keyLen,
                AadLen = aadLen,
                PtLen = ptLen,
                IvLen = ivLen,
                TagLen = tagLen
            };

            var result = _subject.BuildTestGroups(p);

            // Only the min/max aadLen and ptLens go into the creation of the group
            int aadLenMultiplier = (_subject.AadLens.Min() == _subject.AadLens.Max()) ? 1 : 2;
            int ptLenMultiplier = (_subject.PtLens.Min() == _subject.PtLens.Max()) ? 1 : 2;

            int expectedResultCount = _subject.KeyLens.Length * aadLenMultiplier * ptLenMultiplier * _subject.NonceLens.Count() * _subject.TagLens.Count();

            Assert.AreEqual(expectedResultCount, result.Count(c => c.TestType == testType.ToString()));
        }

        /// <summary>
        /// VariableAssocatedData creates test cases based on:
        /// 
        ///     - Each key size
        ///     - Each AAD length (including 2^16)
        ///     - The maximum Payload length
        ///     - The maximum nonce length
        ///     - The maximum tag length
        /// </summary>
        [Test]
        [TestCaseSource(nameof(GetTestParameterData))]
        public void ShouldHaveValidNumberOfGroupsForVariableAssocatedData(
            string testLabel,
            int[] keyLen,
            MathDomain aadLen,
            MathDomain ptLen,
            MathDomain ivLen,
            MathDomain tagLen
        )
        {
            TestTypes testType = TestTypes.VariableAssociatedData;

            Parameters p = new Parameters()
            {
                Algorithm = "AES CCM",
                KeyLen = keyLen,
                AadLen = aadLen,
                PtLen = ptLen,
                IvLen = ivLen,
                TagLen = tagLen
            };

            var result = _subject.BuildTestGroups(p);

            // AAD is a range of values, add an additional group if SupportsAad2Pow16
            int aadLenRange = (_subject.AadLens.Max() - _subject.AadLens.Min()) / 8 + 1 +
                              (_subject.Supports2pow16bytes ? 1 : 0);

            // pt, tag, and nonce use the max value, will always be a multiplier of 1
            int expectedResultCount = keyLen.Length * aadLenRange * 1 * 1 * 1;

            Assert.AreEqual(expectedResultCount, result.Count(c => c.TestType == testType.ToString()));
        }

        /// <summary>
        /// VariableNonce creates test cases based on:
        /// 
        ///     - Each key size
        ///     - The maximum AAD length (excluding 2^16)
        ///     - The maximum Payload length
        ///     - Each nonce length
        ///     - The maximum tag length
        /// 
        /// </summary>
        [Test]
        [TestCaseSource(nameof(GetTestParameterData))]
        public void ShouldHaveValidNumberOfGroupsForVariableNonce(
            string testLabel,
            int[] keyLen,
            MathDomain aadLen,
            MathDomain ptLen,
            MathDomain ivLen,
            MathDomain tagLen
        )
        {
            TestTypes testType = TestTypes.VariableNonce;

            Parameters p = new Parameters()
            {
                Algorithm = "AES CCM",
                KeyLen = keyLen,
                AadLen = aadLen,
                PtLen = ptLen,
                IvLen = ivLen,
                TagLen = tagLen
            };

            var result = _subject.BuildTestGroups(p);

            // aad, pt, and tag use the max value, will always be a multiplier of 1
            int expectedResultCount = keyLen.Length * 1 * _subject.NonceLens.Count() * 1 * 1;

            Assert.AreEqual(expectedResultCount, result.Count(c => c.TestType == testType.ToString()));
        }

        /// <summary>
        /// VariablePayload creates test cases based on:
        /// 
        ///     - Each key size
        ///     - The maximum AAD length (excluding 2^16)
        ///     - Each Payload length
        ///     - The maximum nonce length
        ///     - The maximum tag length
        /// 
        /// </summary>
        [Test]
        [TestCaseSource(nameof(GetTestParameterData))]
        public void ShouldHaveValidNumberOfGroupsForVariablePayload(
            string testLabel,
            int[] keyLen,
            MathDomain aadLen,
            MathDomain ptLen,
            MathDomain ivLen,
            MathDomain tagLen
        )
        {
            TestTypes testType = TestTypes.VariablePayload;

            Parameters p = new Parameters()
            {
                Algorithm = "AES CCM",
                KeyLen = keyLen,
                AadLen = aadLen,
                PtLen = ptLen,
                IvLen = ivLen,
                TagLen = tagLen
            };

            var result = _subject.BuildTestGroups(p);

            // Payload is a range of values, add an additional group if SupportsAad2Pow16
            var payloadCount = (_subject.PtLens.Max() - _subject.PtLens.Min()) / 8 + 1;

            // aad, nonce, and tag use the max value, will always be a multiplier of 1
            int expectedResultCount = keyLen.Length * 1 * payloadCount * 1 * 1;

            Assert.AreEqual(expectedResultCount, result.Count(c => c.TestType == testType.ToString()));
        }

        /// <summary>
        /// VariableTag creates test cases based on:
        /// 
        ///     - Each key size
        ///     - The maximum AAD length (excluding 2^16)
        ///     - The maximum Payload length
        ///     - The maximum nonce length
        ///     - Each tag length
        /// 
        /// </summary>
        [Test]
        [TestCaseSource(nameof(GetTestParameterData))]
        public void ShouldHaveValidNumberOfGroupsForVariableTag(
            string testLabel,
            int[] keyLen,
            MathDomain aadLen,
            MathDomain ptLen,
            MathDomain ivLen,
            MathDomain tagLen
        )
        {
            TestTypes testType = TestTypes.VariableTag;

            Parameters p = new Parameters()
            {
                Algorithm = "AES CCM",
                KeyLen = keyLen,
                AadLen = aadLen,
                PtLen = ptLen,
                IvLen = ivLen,
                TagLen = tagLen
            };

            var result = _subject.BuildTestGroups(p);

            // aad, pt, and nonce use the max value, will always be a multiplier of 1
            int expectedResultCount = keyLen.Length * 1 * 1 * 1 * _subject.TagLens.Length;

            Assert.AreEqual(expectedResultCount, result.Count(c => c.TestType == testType.ToString()));
        }

        [Test]
        [TestCase(TestTypes.DecryptionVerification, true, false)]
        [TestCase(TestTypes.VariableAssociatedData, true, true)]
        [TestCase(TestTypes.VariableNonce, true, false)]
        [TestCase(TestTypes.VariablePayload, true, true)]
        [TestCase(TestTypes.VariableTag, true, true)]
        public void ShouldUseSharedParameterInGroup(TestTypes testType, bool useSharedKey, bool useSharedNonce)
        {
            Parameters p = new ParameterBuilder().Build();

            var result = _subject.BuildTestGroups(p);

            var typeTests = result.Where(w => w.TestType == testType.ToString()).Select(s => (TestGroup)s);
            var correctShares =
                typeTests.All(
                    a =>
                        a.GroupReusesKeyForTestCases == useSharedKey &&
                        a.GroupReusesNonceForTestCases == useSharedNonce);

            Assert.IsTrue(correctShares);
        }
    }
}