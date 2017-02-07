using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework.Internal;
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
            var result = subject.BuildTestVectorSet(new Parameters {Mode = new[] {"SHA1"}, DigestSize = new[] {"160"}});
            Assert.IsNotNull(result);
        }

        [Test]
        public void ShouldReturnVectorSetWithProperTestGroupsForAllModes()
        {
            var subject = new TestVectorFactory();
            var result = subject.BuildTestVectorSet(new Parameters {Mode = new[] {"SHA1", "SHA2"}, DigestSize = new[]{ "160", "224", "256", "384", "512", "512t224", "512t256" } });
            Assume.That(result != null);
            Assert.AreEqual(42, result.TestGroups.Count);       // 2 * 7 * 3
        }
    }
}
