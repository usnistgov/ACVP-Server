using NUnit.Framework;

namespace NIST.CVP.Generation.SHA2.Tests
{
    [TestFixture]
    public class TestVectorFactoryTests
    {
        [Test]
        public void ShouldReturnVectorSet()
        {
            var subject = new TestVectorFactory();
            var result = subject.BuildTestVectorSet(
                new Parameters
                {
                    Algorithm = "SHA",
                    Functions = new []
                    {
                        new Function
                        {
                            Mode = "sha1",
                            DigestSizes = new [] {"160"}
                        }
                    },
                    BitOriented = true,
                    IncludeNull = true,
                }
            );

            Assert.IsNotNull(result);
        }

        [Test]
        public void ShouldReturnVectorSetWithProperTestGroupsForAllModes()
        {
            var subject = new TestVectorFactory();
            var result = subject.BuildTestVectorSet(
                new Parameters
                {
                    Algorithm = "SHA",
                    Functions = new []
                    {
                        new Function
                        {
                            Mode = "sha1",
                            DigestSizes = new [] {"160"}
                        },
                        new Function
                        {
                            Mode = "sha2",
                            DigestSizes = new [] {"224", "256", "384", "512", "512/224", "512/256"}
                        }
                    },
                    BitOriented = true,
                    IncludeNull = true,
                }
            );

            Assume.That(result != null);
            Assert.AreEqual(14, result.TestGroups.Count);       // 2 * 7 (aft + mct X digest sizes)
        }
    }
}
