using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.SHA2.Tests
{
    [TestFixture, UnitTest]
    public class TestVectorFactoryTests
    {
        [Test]
        public void ShouldReturnVectorSet()
        {
            var subject = new TestVectorFactory(new AFTTestGroupFactory(), new MCTTestGroupFactory());
            var result = subject.BuildTestVectorSet(
                new Parameters
                {
                    Algorithm = "SHA1",
                    DigestSizes = new [] {"160"},
                    BitOriented = true,
                    IncludeNull = true,
                }
            );

            Assert.IsNotNull(result);
        }

        [Test]
        public void ShouldReturnVectorSetWithProperTestGroupsForAllModes()
        {
            var subject = new TestVectorFactory(new AFTTestGroupFactory(), new MCTTestGroupFactory());
            var result = subject.BuildTestVectorSet(
                new Parameters
                {
                    Algorithm = "SHA2",
                    DigestSizes = new [] {"224", "256", "384", "512", "512/224", "512/256"},
                    BitOriented = true,
                    IncludeNull = true,
                }
            );

            Assume.That(result != null);
            Assert.AreEqual(12, result.TestGroups.Count);       // 2 * 6 (aft + mct X digest sizes)
        }
    }
}
