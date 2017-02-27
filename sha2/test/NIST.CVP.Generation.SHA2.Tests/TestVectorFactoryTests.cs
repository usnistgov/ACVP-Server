using NUnit.Framework;

namespace NIST.CVP.Generation.SHA2.Tests
{
    [TestFixture]
    public class TestVectorFactoryTests
    {
        [Test]
        public void ShouldReturnVectorSet()
        {
            var subject = new TestVectorFactory(new MCTTestGroupFactory());
            var result = subject.BuildTestVectorSet(new Parameters {Mode = new[] {"SHA1"}, DigestSize = new[] {"160"}});
            Assert.IsNotNull(result);
        }

        [Test]
        public void ShouldReturnVectorSetWithProperTestGroupsForAllModes()
        {
            var subject = new TestVectorFactory(new MCTTestGroupFactory());
            var result = subject.BuildTestVectorSet(new Parameters {Mode = new[] {"SHA1", "SHA2"}, DigestSize = new[]{ "160", "224", "256", "384", "512", "512t224", "512t256" } });
            Assume.That(result != null);
            Assert.AreEqual(21, result.TestGroups.Count);       // 3 * 7 (short long mct X digest sizes)
        }
    }
}
