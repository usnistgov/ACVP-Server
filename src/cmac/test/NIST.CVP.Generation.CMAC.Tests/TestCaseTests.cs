﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.CMAC.AES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.CMAC_AES.Tests
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
        [TestCase("key")]
        [TestCase("msg")]
        [TestCase("mac")]
        public void ShouldSetNullValues(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, null);
            Assert.IsTrue(result);
            
        }

        [Test]
        [TestCase("key")]
        [TestCase("KEY")]
        public void ShouldSetKey(string name)
        {
            const string value = "00AA";

            var subject = new TestCase();
            var result = subject.SetString(name, value);
            Assert.IsTrue(result);
            Assert.AreEqual(value, subject.Key.ToHex());
        }
        
        [Test]
        [TestCase("msg")]
        [TestCase("MsG")]
        public void ShouldSetMessage(string name)
        {
            const string value = "00AA";

            var subject = new TestCase();
            var result = subject.SetString(name, value);
            Assert.IsTrue(result);
            Assert.AreEqual(value, subject.Message.ToHex());
        }

        [Test]
        [TestCase("mac")]
        [TestCase("MaC")]
        public void ShouldSetMac(string name)
        {
            const string value = "00AA";

            var subject = new TestCase();
            var result = subject.SetString(name, value);
            Assert.IsTrue(result);
            Assert.AreEqual(value, subject.Mac.ToHex());
        }
    }
}
