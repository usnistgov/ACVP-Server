﻿using System;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.SSH;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.SSH
{
    [TestFixture, UnitTest]
    public class TestGroupTests
    {
        [Test]
        [TestCase("Fredo")]
        [TestCase("")]
        [TestCase("NULL")]
        [TestCase(null)]
        public void ShouldReturnFalseIfUnknownSetStringName(string name)
        {
            var subject = new TestGroup();
            var result = subject.SetString(name, "1");
            Assert.That(result, Is.False);
        }

        [Test]
        public void ShouldReturnFalseIfPassObjectCannotCast()
        {
            var subject = new TestGroup();
            var result = subject.Equals(null);

            Assert.That(result, Is.False);
        }

        [Test]
        [TestCase("hashAlg")]
        [TestCase("HASHALG")]
        public void ShouldSetHashAlg(string name)
        {
            const string shaValue = "sha2-256";

            var subject = new TestGroup();

            var result = subject.SetString(name, shaValue);
            Assert.That(result, Is.True, nameof(result));
            Assert.That(shaValue.Equals(subject.HashAlg.Name, StringComparison.OrdinalIgnoreCase), Is.True, nameof(subject.HashAlg.Name));
        }
    }
}
