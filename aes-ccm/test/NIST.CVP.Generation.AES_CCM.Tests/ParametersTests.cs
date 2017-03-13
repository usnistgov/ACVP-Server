using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_CCM.Tests
{
    [TestFixture]
    public class ParametersTests
    {
        private ParameterBuilder _subjectBuilder = new ParameterBuilder();
        
        [Test]
        [TestCase("0 through 32", new[] { 0, 32 })]
        [TestCase("0 through 50", new[] { 0, 32 })]
        [TestCase("0 through 50", new[] { 0, 50 })]
        [TestCase("0 through 50000", new[] { 0, 50000 })]
        public void SupportsAad2Pow16ShouldBeFalseWhenAadDoesNotContain65536(string testLabel, int[] aadLen)
        {
            var subject = _subjectBuilder
                .WithAadLen(
                    new Range()
                    {
                        Min = aadLen[0],
                        Max = aadLen[1]
                    }
                )
                .Build();

            Assert.IsFalse(subject.SupportsAad2Pow16);
        }

        [Test]
        [TestCase("0 through 65536", new[] { 0, 65536 })]
        [TestCase("32 through 65536", new[] { 32, 65536 })]
        [TestCase("50 through 65536", new[] { 50, 65536 })]
        [TestCase("65536 through 65536", new[] { 65536, 65536 })]
        public void SupportsAad2Pow16ShouldBeTrueWhenAadDoesContain65536(string testLabel, int[] aadLen)
        {
            var subject = _subjectBuilder
                .WithAadLen(
                    new Range()
                    {
                        Min = aadLen[0],
                        Max = aadLen[1]
                    }
                )
                .Build();

            Assert.IsTrue(subject.SupportsAad2Pow16);
        }

        [Test]
        [TestCase("0 to 1, no changes", new[] { 0, 1 })]
        [TestCase("0 to 32, no changes", new[] { 0, 32 })]
        [TestCase("0 to 33, 33 changes", new[] { 0, 33})]
        [TestCase("35 to 65536, both change", new[] { 35, 65536 })]
        public void ShouldChangeValuesGt32To32(string testLabel, int[] aadLen)
        {
            var subject = _subjectBuilder
                .WithAadLen(
                    new Range()
                    {
                        Min = aadLen[0],
                        Max = aadLen[1]
                    }
                )
                .Build();

            if (aadLen[0] <= 32)
            {
                Assert.AreEqual(aadLen[0], subject.AadLen.Min);
            }
            else
            {
                Assert.AreEqual(32, subject.AadLen.Min);
            }

            if (aadLen[1] <= 32)
            {
                Assert.AreEqual(aadLen[1], subject.AadLen.Max);
            }
            else
            {
                Assert.AreEqual(32, subject.AadLen.Max);
            }
        }
    }
}
