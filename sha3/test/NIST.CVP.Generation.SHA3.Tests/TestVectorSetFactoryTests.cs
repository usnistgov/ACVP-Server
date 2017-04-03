using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NIST.CVP.Generation.SHA3.Tests
{
    [TestFixture]
    public class TestVectorSetFactoryTests
    {
        [Test]
        public void ShouldReturnVectorSet()
        {
            var subject = new TestVectorFactory();
            var result = subject.BuildTestVectorSet(
                new Parameters
                {
                    Function = new [] {"SHA3", "SHA3", "SHA3", "SHA3", "SHAKE", "SHAKE"},
                    DigestSize = new [] {224, 256, 384, 512, 128, 256}
                });
            Assert.IsNotNull(result);
        }

        [Test]
        public void ShouldReturnVectorSetWithProperTestGroupsForAllModes()
        {
            var subject = new TestVectorFactory();
            var result = subject.BuildTestVectorSet(
                new Parameters
                {
                    Function = new[] { "SHA3", "SHA3", "SHA3", "SHA3", "SHAKE", "SHAKE" },
                    DigestSize = new[] { 224, 256, 384, 512, 128, 256 }
                }); Assume.That(result != null);
            Assert.AreEqual(14, result.TestGroups.Count);       // 6 * 3 - 4 (digest sizes * test types - sha3 can't do vot)
        }
    }
}
