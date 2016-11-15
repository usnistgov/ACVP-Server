using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_GCM.Tests
{
    [TestFixture]
    public class TestVectorFactoryTests
    {
        private int[] _aadLengths =  {16, 32, 48};
        private int[] _keyLengths =  { 128, 194, 256 };
        private int[] _ivLengths = { 96, 128 };
        private int[] _tagLengths =  { 20, 24, 28, 32 };
        private int[] _ptLengths =  { 128, 192, 256, 512 };
        private string[] _modes = {"encrypt", "decrypt"};

        [Test]
        public void ShouldBuildTestVector()
        {
            var subject = new TestVectorFactory();
            var result = subject.BuildTestVectorSet(GetParameters(new[] {"encrypt"}));
            Assert.IsNotNull(result);
        }

        private Parameters GetParameters(string[] mode)
        {
            return new Parameters
            {
                aadLen = new[] {16},
                KeyLen = new[] {128},
                ivLen = new[] {96},
                Mode = mode,
                PtLen = new[] {512},
                TagLen = new[] {32}
            };
        }

        [Test]
        [TestCase(new []{ "encrypt" }, 1)]
        [TestCase(new[] { "decrypt" }, 1)]
        [TestCase(new[] { "encrypt", "decrypt" }, 1)]
        public void ShouldBuildTestVectorWithGroupsContainingSuppliedModes(string[] modes, int expectedGroupsPerMode)
        {
            var subject = new TestVectorFactory();
            var result = subject.BuildTestVectorSet(GetParameters(modes));
            Assume.That(result != null);
            foreach (var mode in modes)
            {
                Assert.AreEqual(expectedGroupsPerMode, result.TestGroups.Count(m => ((TestGroup)m).Function == mode));
            }
            Assert.AreEqual(modes.Length, result.TestGroups.Count);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void ShouldHaveGroupsWithSuppliedKeyLengths(int numberOfKeyLengths)
        {
            int[] keyLengths = new int[numberOfKeyLengths];
            Array.Copy(_keyLengths, keyLengths, numberOfKeyLengths);
            var parameters = new Parameters
            {
                aadLen = _aadLengths,
                KeyLen = keyLengths,
                ivLen = _ivLengths,
                Mode =_modes,
                PtLen = _ptLengths,
                TagLen = _tagLengths
            };
            var subject = new TestVectorFactory();
            var result = subject.BuildTestVectorSet(parameters);
            int expectedGroupsPerLength = _tagLengths.Length*_modes.Length*_ivLengths.Length*_ptLengths.Length*
                                          _aadLengths.Length;

            foreach (var length in keyLengths)
            {
                var groups = result.TestGroups.Where(g => ((TestGroup) g).KeyLength == length);
                Assert.AreEqual(expectedGroupsPerLength, groups.Count());
            }
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void ShouldHaveGroupsWithSuppliedAADLengths(int numberOfAADLengths)
        {
            int[] aadlengths = new int[numberOfAADLengths];
            Array.Copy(_aadLengths, aadlengths, numberOfAADLengths);
            var parameters = new Parameters
            {
                aadLen = aadlengths,
                KeyLen = _keyLengths,
                ivLen = _ivLengths,
                Mode = _modes,
                PtLen = _ptLengths,
                TagLen = _tagLengths
            };
            var subject = new TestVectorFactory();
            var result = subject.BuildTestVectorSet(parameters);
            int expectedGroupsPerLength = _tagLengths.Length * _modes.Length * _ivLengths.Length * _ptLengths.Length *
                                          _keyLengths.Length;

            foreach (var length in aadlengths)
            {
                var groups = result.TestGroups.Where(g => ((TestGroup)g).AADLength == length);
                Assert.AreEqual(expectedGroupsPerLength, groups.Count());
            }
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public void ShouldHaveGroupsWithSuppliedIVLengths(int numberOfIVLengths)
        {
            int[] ivLengths = new int[numberOfIVLengths];
            Array.Copy(_ivLengths, ivLengths, numberOfIVLengths);
            var parameters = new Parameters
            {
                aadLen = _aadLengths,
                KeyLen = _keyLengths,
                ivLen = ivLengths,
                Mode = _modes,
                PtLen = _ptLengths,
                TagLen = _tagLengths
            };
            var subject = new TestVectorFactory();
            var result = subject.BuildTestVectorSet(parameters);
            int expectedGroupsPerLength = _tagLengths.Length * _modes.Length * _aadLengths.Length * _ptLengths.Length *
                                          _keyLengths.Length;

            foreach (var length in ivLengths)
            {
                var groups = result.TestGroups.Where(g => ((TestGroup)g).IVLength == length);
                Assert.AreEqual(expectedGroupsPerLength, groups.Count());
            }
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void ShouldHaveGroupsWithSuppliedPTLengths(int numberOfPTLengths)
        {
            int[] ptLengths = new int[numberOfPTLengths];
            Array.Copy(_ptLengths, ptLengths, numberOfPTLengths);
            var parameters = new Parameters
            {
                aadLen = _aadLengths,
                KeyLen = _keyLengths,
                ivLen = _ivLengths,
                Mode = _modes,
                PtLen = ptLengths,
                TagLen = _tagLengths
            };
            var subject = new TestVectorFactory();
            var result = subject.BuildTestVectorSet(parameters);
            int expectedGroupsPerLength = _tagLengths.Length * _modes.Length * _aadLengths.Length * _ivLengths.Length *
                                          _keyLengths.Length;

            foreach (var length in ptLengths)
            {
                var groups = result.TestGroups.Where(g => ((TestGroup)g).PTLength == length);
                Assert.AreEqual(expectedGroupsPerLength, groups.Count());
            }
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void ShouldHaveGroupsWithSuppliedTagLengths(int numberOfTagLengths)
        {
            int[] tagLengths = new int[numberOfTagLengths];
            Array.Copy(_tagLengths, tagLengths, numberOfTagLengths);
            var parameters = new Parameters
            {
                aadLen = _aadLengths,
                KeyLen = _keyLengths,
                ivLen = _ivLengths,
                Mode = _modes,
                PtLen = _ptLengths,
                TagLen = tagLengths
            };
            var subject = new TestVectorFactory();
            var result = subject.BuildTestVectorSet(parameters);
            int expectedGroupsPerLength = _ptLengths.Length * _modes.Length * _aadLengths.Length * _ivLengths.Length *
                                          _keyLengths.Length;

            foreach (var length in tagLengths)
            {
                var groups = result.TestGroups.Where(g => ((TestGroup)g).TagLength == length);
                Assert.AreEqual(expectedGroupsPerLength, groups.Count());
            }
        }
    }
}
