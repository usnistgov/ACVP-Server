﻿using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.TPMv1_2;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.TPM
{
    [TestFixture, UnitTest]
    public class TestCaseTests
    {
        [Test]
        [TestCase("Fredo")]
        [TestCase("")]
        [TestCase("NULL")]
        [TestCase(null)]
        public void ShouldReturnFalseIfUnknownSetStringName(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, "00AA");
            Assert.IsFalse(result);
        }

        [Test]
        [TestCase("auth")]
        [TestCase("AUTH")]
        public void ShouldSetAuth(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, "00AA");
            Assert.IsTrue(result);
            Assert.AreEqual("00AA", subject.Auth.ToHex());
        }

        [Test]
        [TestCase("Nonce_Even")]
        [TestCase("nonce_even")]
        [TestCase("NONCE_EVEN")]
        public void ShouldSetNonceEven(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, "00AA");
            Assert.IsTrue(result);
            Assert.AreEqual("00AA", subject.NonceEven.ToHex());
        }

        [Test]
        [TestCase("Nonce_Odd")]
        [TestCase("nonce_odd")]
        [TestCase("NONCE_ODD")]
        public void ShouldSetNonceOdd(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, "00AA");
            Assert.IsTrue(result);
            Assert.AreEqual("00AA", subject.NonceOdd.ToHex());
        }
    }
}
