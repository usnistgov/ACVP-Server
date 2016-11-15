using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using NIST.CVP.Math;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_GCM.Tests
{
    [TestFixture]
    public class TestCaseGeneratorExternalEncryptTests
    {
        private  Random800_90 _randy = new Random800_90();
        [Test]
        public void ShouldReturnSuccessForInitialGenerate()
        {
            var subject = GetSubject();
            var result = subject.Generate(GetTestGroup());
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldReturnSuppliedCipherTextLengthForGenerate()
        {
            var subject = GetSubject();
            var group = GetTestGroup();
            var result = subject.Generate(group);
            Assume.That(result.Success);
            var testCase = (TestCase) result.TestCase;
            Assert.AreEqual(group.PTLength, testCase.CipherText.Length);
        }

        [Test]
        public void ShouldReturnSuppliedTagLengthForGenerate()
        {
            var subject = GetSubject();
            var group = GetTestGroup();
            var result = subject.Generate(group);
            Assume.That(result.Success);
            var testCase = (TestCase)result.TestCase;
            Assert.AreEqual(group.TagLength, testCase.Tag.Length);
        }

        [Test]
        public void ShouldReturnSuppliedCipherTextLengthForRegenerate()
        {
            var subject = GetSubject();
            var group = GetTestGroup();
            var testCaseProto = GetRegenerateTestCase(group);

            var result = subject.Generate(group, testCaseProto);
            Assume.That(result.Success);
            var testCase = (TestCase)result.TestCase;
            Assert.AreEqual(group.PTLength, testCase.CipherText.Length);
        }

        [Test]
        public void ShouldReturnSuppliedTagLengthForRegenerate()
        {
            var subject = GetSubject();
            var group = GetTestGroup();
            var testCaseProto = GetRegenerateTestCase(group);

            var result = subject.Generate(group, testCaseProto);
            Assume.That(result.Success);
            var testCase = (TestCase)result.TestCase;
            Assert.AreEqual(group.TagLength, testCase.Tag.Length);
        }

        private TestCase GetRegenerateTestCase(TestGroup group)
        {
            var testCase = new TestCase
            {
                PlainText = _randy.GetRandomBitString(group.PTLength),
                Tag =  _randy.GetRandomBitString(group.TagLength),
                Key = _randy.GetRandomBitString(group.KeyLength),
                AAD = _randy.GetRandomBitString(group.AADLength),
                IV = _randy.GetRandomBitString(group.IVLength)

            };
            return testCase;
        }


        [Test]
        public void ShouldHaveEncryptForDirection()
        {
            var subject = GetSubject();

            Assert.AreEqual("encrypt", subject.Direction);
        }

        [Test]
        public void ShouldHaveExternalForIVGen()
        {
            var subject = GetSubject();

            Assert.AreEqual("external", subject.IVGen);
        }

        private TestGroup GetTestGroup()
        {
            return  new TestGroup { AADLength = 16, Function = "Encrypt", IVGeneration = "External", KeyLength = 128, PTLength = 512, TagLength = 32};
        }

        private TestCaseGeneratorExternalEncrypt GetSubject()
        {
            return  new TestCaseGeneratorExternalEncrypt(_randy, new AES_GCM());
        }
    }
}
