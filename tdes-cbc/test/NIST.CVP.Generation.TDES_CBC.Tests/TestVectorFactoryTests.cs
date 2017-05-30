using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_CBC.Tests
{
    [TestFixture, UnitTest]
    public class TestVectorFactoryTests
    {
        [Test]
        public void ShouldReturnVectorSet()
        {
            var subject = new TestVectorFactory();
            var result = subject.BuildTestVectorSet(
                new Parameters
                {
                    Mode = new[] { "encrypt" },
                    KeyingOption = new[] { 1, 2 }
                });
            Assert.IsNotNull(result);
        }

        [Test]
        [TestCase("Permutation", 1)]
        [TestCase("InversePermutation", 1)]
        [TestCase("SubstitutionTable", 1)]
        [TestCase("VariableKey", 1)]
        [TestCase("VariableText", 1)]
        [TestCase("MultiBlockMessage", 3)]
        [TestCase("MCT", 3)]
        public void ShouldReturnVectorSetWithProperEncryptionTestGroups(string testType, int keyCount)
        {
            var subject = new TestVectorFactory();
            var result = subject.BuildTestVectorSet(
                new Parameters
                {
                    Mode = new[] { "encrypt" },
                    KeyingOption = new[] { 1, 2 }
                });
            Assume.That(result != null);
            Assert.AreEqual(7, result.TestGroups.Count);
            Assert.IsNotNull(result.TestGroups.First(tg => tg.TestType == testType && ((TestGroup)tg).NumberOfKeys == keyCount && ((TestGroup)tg).Function == "encrypt"));
        }

        [Test]
        [TestCase("Permutation", 1)]
        [TestCase("InversePermutation", 1)]
        [TestCase("SubstitutionTable", 1)]
        [TestCase("VariableKey", 1)]
        [TestCase("VariableText", 1)]
        [TestCase("MultiBlockMessage", 2)]
        [TestCase("MCT", 2)]
        [TestCase("MultiBlockMessage", 3)]
        [TestCase("MCT", 3)]
        public void ShouldReturnVectorSetWithProperDecryptionTestGroups(string testType, int keyCount)
        {
            var subject = new TestVectorFactory();
            var result = subject.BuildTestVectorSet(
                new Parameters
                {
                    Mode = new[] { "decrypt" },
                    KeyingOption = new[] { 1, 2 }
                });
            Assume.That(result != null);
            Assert.AreEqual(14, result.TestGroups.Count);
            Assert.IsNotNull(result.TestGroups.First(tg => tg.TestType == testType && ((TestGroup)tg).NumberOfKeys == keyCount && ((TestGroup)tg).Function == "decrypt"));
        }

        [Test]
        public void ShouldReturnVectorSetWithProperTestGroupsForBothDirections()
        {
            var subject = new TestVectorFactory();
            var result = subject.BuildTestVectorSet(
                new Parameters
                {
                    Mode = new[] { "encrypt", "decrypt" },
                    KeyingOption = new[] { 1, 2 }
                });
            Assume.That(result != null);
            Assert.AreEqual(21, result.TestGroups.Count);

        }

    }
}
