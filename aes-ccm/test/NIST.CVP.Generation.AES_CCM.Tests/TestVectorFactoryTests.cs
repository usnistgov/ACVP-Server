using System;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CCM.Tests
{
    [TestFixture]
    public class TestVectorFactoryTests
    {

        #region Parameter test scenarios
        private static object[] testParameterData = new[]
        {
            new object[]
            {
                "minimum inputs",
                new[] {128},
                new Range() { Min = 0, Max = 0 },
                new Range() { Min = 0, Max = 0 },
                new[] {7},
                new[] {4}
            },
            new object[]
            {
                "multiple inputs, single array",
                new[] {128, 192, 256},
                new Range() { Min = 0, Max = 0 },
                new Range() { Min = 0, Max = 0 },
                new[] {7},
                new[] {4}
            },
            new object[]
            {
                "multiple differing inputs, in min/max array",
                new[] {128},
                new Range() { Min = 0, Max = 32 },
                new Range() { Min = 0, Max = 0 },
                new[] {7},
                new[] {4}
            },
            new object[]
            {
                "max number of groups, no 2^16",
                new[] {128, 192, 256},
                new Range() { Min = 0, Max = 32 },
                new Range() { Min = 0, Max = 32 },
                new[] {7, 8, 9, 10, 11, 12, 13},
                new[] {4, 6, 8, 10, 12, 14, 16}
            },
            new object[]
            {
                "max number of groups, max aad",
                new[] {128, 192, 256},
                new Range() { Min = 0, Max = 65536 },
                new Range() { Min = 0, Max = 32 },
                new[] {7, 8, 9, 10, 11, 12, 13},
                new[] {4, 6, 8, 10, 12, 14, 16}
            }
        };
        #endregion Parameter test scenarios

        [Test]
        public void ShouldContainGroupsForEachTestType()
        {
            Parameters p = new Parameters()
            {
                Algorithm = "AES-CCM",
                KeyLen = new[] { 128 },
                AadLen = new Range() { Min = 0, Max = 32 },
                PtLen = new Range() { Min = 0, Max = 32 },
                Nonce = new [] { 7 },
                TagLen = new [] { 4 },
            };

            TestVectorFactory subject = new TestVectorFactory();
            var result = subject.BuildTestVectorSet(p);

            foreach (TestTypes testType in Enum.GetValues(typeof(TestTypes)))
            {
                Assert.IsTrue(result.TestGroups.Any(a => a.TestType == testType.ToString()));
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
        [TestCaseSource(nameof(testParameterData))]
        public void ShouldHaveValidNumberOfGroupsForDecryptionVerification(
            string testLabel,
            int[] keyLen,
            Range aadLen,
            Range ptLen,
            int[] ivLen,
            int[] tagLen
        )
        {
            TestTypes testType = TestTypes.DecryptionVerification;

            Parameters p = new Parameters()
            {
                Algorithm = "AES CCM",
                KeyLen = keyLen,
                AadLen = aadLen,
                PtLen = ptLen,
                Nonce = ivLen,
                TagLen = tagLen
            };

            // Only the min/max aadLen and ptLens go into the creation of the group
            int aadLenMultiplier = (aadLen.Min == aadLen.Max) ? 1 : 2;
            int ptLenMultiplier = (ptLen.Min == ptLen.Max) ? 1 : 2;

            int expectedResultCount = keyLen.Length * aadLenMultiplier * ptLenMultiplier * ivLen.Length * tagLen.Length;
            
            TestVectorFactory subject = new TestVectorFactory();
            var result = subject.BuildTestVectorSet(p);

            Assert.AreEqual(expectedResultCount, result.TestGroups.Count(c => c.TestType == testType.ToString()));
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
        [TestCaseSource(nameof(testParameterData))]
        public void ShouldHaveValidNumberOfGroupsForVariableAssocatedData(
            string testLabel,
            int[] keyLen,
            Range aadLen,
            Range ptLen,
            int[] ivLen,
            int[] tagLen
        )
        {
            TestTypes testType = TestTypes.VariableAssociatedData;

            Parameters p = new Parameters()
            {
                Algorithm = "AES CCM",
                KeyLen = keyLen,
                AadLen = aadLen,
                PtLen = ptLen,
                Nonce = ivLen,
                TagLen = tagLen
            };

            // AAD is a range of values, add an additional group if SupportsAad2Pow16
            int aadLenRange = aadLen.Max - aadLen.Min + 1 + (p.SupportsAad2Pow16 ? 1 : 0);

            // pt, tag, and nonce use the max value, will always be a multiplier of 1
            int expectedResultCount = keyLen.Length * aadLenRange * 1 * 1 * 1;

            TestVectorFactory subject = new TestVectorFactory();
            var result = subject.BuildTestVectorSet(p);

            Assert.AreEqual(expectedResultCount, result.TestGroups.Count(c => c.TestType == testType.ToString()));
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
        [TestCaseSource(nameof(testParameterData))]
        public void ShouldHaveValidNumberOfGroupsForVariableNonce(
            string testLabel,
            int[] keyLen,
            Range aadLen,
            Range ptLen,
            int[] ivLen,
            int[] tagLen
        )
        {
            TestTypes testType = TestTypes.VariableNonce;

            Parameters p = new Parameters()
            {
                Algorithm = "AES CCM",
                KeyLen = keyLen,
                AadLen = aadLen,
                PtLen = ptLen,
                Nonce = ivLen,
                TagLen = tagLen
            };

            // aad, pt, and tag use the max value, will always be a multiplier of 1
            int expectedResultCount = keyLen.Length * 1 * ivLen.Length * 1 * 1;

            TestVectorFactory subject = new TestVectorFactory();
            var result = subject.BuildTestVectorSet(p);

            Assert.AreEqual(expectedResultCount, result.TestGroups.Count(c => c.TestType == testType.ToString()));
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
        [TestCaseSource(nameof(testParameterData))]
        public void ShouldHaveValidNumberOfGroupsForVariablePayload(
            string testLabel,
            int[] keyLen,
            Range aadLen,
            Range ptLen,
            int[] ivLen,
            int[] tagLen
        )
        {
            TestTypes testType = TestTypes.VariablePayload;

            Parameters p = new Parameters()
            {
                Algorithm = "AES CCM",
                KeyLen = keyLen,
                AadLen = aadLen,
                PtLen = ptLen,
                Nonce = ivLen,
                TagLen = tagLen
            };

            // Payload is a range of values, add an additional group if SupportsAad2Pow16
            var payloadCount = ptLen.Max - ptLen.Min + 1;

            // aad, nonce, and tag use the max value, will always be a multiplier of 1
            int expectedResultCount = keyLen.Length * 1 * payloadCount * 1 * 1;

            TestVectorFactory subject = new TestVectorFactory();
            var result = subject.BuildTestVectorSet(p);

            Assert.AreEqual(expectedResultCount, result.TestGroups.Count(c => c.TestType == testType.ToString()));
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
        [TestCaseSource(nameof(testParameterData))]
        public void ShouldHaveValidNumberOfGroupsForVariableTag(
            string testLabel,
            int[] keyLen,
            Range aadLen,
            Range ptLen,
            int[] ivLen,
            int[] tagLen
        )
        {
            TestTypes testType = TestTypes.VariableTag;

            Parameters p = new Parameters()
            {
                Algorithm = "AES CCM",
                KeyLen = keyLen,
                AadLen = aadLen,
                PtLen = ptLen,
                Nonce = ivLen,
                TagLen = tagLen
            };

            // aad, pt, and nonce use the max value, will always be a multiplier of 1
            int expectedResultCount = keyLen.Length * 1 * 1 * 1 * tagLen.Length;

            TestVectorFactory subject = new TestVectorFactory();
            var result = subject.BuildTestVectorSet(p);

            Assert.AreEqual(expectedResultCount, result.TestGroups.Count(c => c.TestType == testType.ToString()));
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

            TestVectorFactory subject = new TestVectorFactory();
            var result = subject.BuildTestVectorSet(p);

            var typeTests = result.TestGroups.Where(w => w.TestType == testType.ToString()).Select(s => (TestGroup)s);
            var correctShares =
                typeTests.All(
                    a =>
                        a.GroupReusesKeyForTestCases == useSharedKey &&
                        a.GroupReusesNonceForTestCases == useSharedNonce);

            Assert.IsTrue(correctShares);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldSetIsSampleProperlyFromTheParameters(bool isSample)
        {
            Parameters p = new Parameters()
            {
                AadLen = new Range { Min = 1, Max = 1 },
                Algorithm = "AES CCM",
                Nonce = new int[] { 1 },
                KeyLen = new int[] { 1 },
                PtLen = new Range { Min = 1, Max = 1 },
                TagLen = new int[] { 1 },
                IsSample = isSample
            };

            TestVectorFactory subject = new TestVectorFactory();
            var result = subject.BuildTestVectorSet(p);

            Assert.AreEqual(isSample, result.IsSample);
        }

    }
}
