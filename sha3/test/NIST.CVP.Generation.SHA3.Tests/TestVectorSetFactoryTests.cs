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
            var subject = new TestVectorFactory(new MCTTestGroupFactory());
            var result = subject.BuildTestVectorSet(
                new Parameters
                {
                    Algorithm = "SHA3",
                    DigestSizes = new [] {224, 256, 384, 512},
                    BitOrientedInput = true,
                    IncludeNull = true,
                    IsSample = false,
                });
            Assert.IsNotNull(result);
        }

        [Test]
        public void ShouldReturnVectorSetWithProperTestGroupsForAllModes()
        {
            var subject = new TestVectorFactory(new MCTTestGroupFactory());
            var result = subject.BuildTestVectorSet(
                new Parameters
                {
                    Algorithm = "SHAKE",
                    DigestSizes = new [] {128, 256},
                    BitOrientedInput = true,
                    BitOrientedOutput = true,
                    IncludeNull = true,
                    IsSample = false,
                    MaxOutputLength = 65536,
                    MinOutputLength = 16
                });
            Assume.That(result != null);
            Assert.AreEqual(6, result.TestGroups.Count);       // 2 * 3 (digest sizes * test types)
        }
    }
}
