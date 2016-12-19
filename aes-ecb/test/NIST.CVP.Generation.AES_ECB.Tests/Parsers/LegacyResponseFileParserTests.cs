using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using NIST.CVP.Generation.AES_ECB.Parsers;
using NIST.CVP.Tests.Core;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_ECB.Tests.Parsers
{
    [TestFixture]
    public class LegacyResponseFileParserTests
    {
        private string _unitTestPath;

        [OneTimeSetUp]
        public void Setup()
        {
            _unitTestPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\LegacyParserFiles\");
        }

        [Test]
        public void ShouldReturnErrorForNonExistentPath()
        {
            LegacyResponseFileParser subject = new LegacyResponseFileParser();
            var result = subject.Parse($@"C:\{Guid.NewGuid()}\testResults.json");
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void ShouldReturnErrorForNullOrEmptyPath(string path)
        {
            LegacyResponseFileParser subject = new LegacyResponseFileParser();
            var result = subject.Parse(path);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
        }

        [Test]
        public void ShouldParseValidFile()
        {
            LegacyResponseFileParser subject = new LegacyResponseFileParser();
            string path = Path.Combine(_unitTestPath, "ECBVarKey128.rsp");
            var result = subject.Parse(path);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldHaveProperNumberOfGroups()
        {
            LegacyResponseFileParser subject = new LegacyResponseFileParser();
            string path = Path.Combine(_unitTestPath, "ECBVarKey128.rsp");
            var result = subject.Parse(path);
            Assume.That(result != null);
            TestVectorSet vectorSet = result.ParsedObject;
            Assert.AreEqual(2, vectorSet.TestGroups.Count);
        }

        [Test]
        public void ShouldHaveProperNumberOfCasesPerGroup()
        {
            LegacyResponseFileParser subject = new LegacyResponseFileParser();
            string path = Path.Combine(_unitTestPath, "ECBVarKey128.rsp");
            var result = subject.Parse(path);
            Assume.That(result != null);
            TestVectorSet vectorSet = result.ParsedObject;
            Assert.IsTrue(vectorSet.TestGroups.All(g => g.Tests.Count == 128));
        }

        [Test]
        [TestCase(128)]
        [TestCase(192)]
        [TestCase(256)]
        public void ShouldParseKeyLengthFromHeader(int keyLen)
        {
            LegacyResponseFileParser subject = new LegacyResponseFileParser();
            string fileName = $"ECBVarKey{keyLen}.rsp";
            string path = Path.Combine(_unitTestPath, fileName);
            var result = subject.Parse(path);
            Assume.That(result != null);
            TestVectorSet vectorSet = result.ParsedObject;
            Assert.IsTrue(vectorSet.TestGroups.All(g => g.KeyLength == keyLen));
        }

        [Test]
        public void ShouldHaveTestsWithKeyFilled()
        {
            LegacyResponseFileParser subject = new LegacyResponseFileParser();
            string path = Path.Combine(_unitTestPath, "ECBVarKey128.rsp");
            var result = subject.Parse(path);
            Assume.That(result != null);
            TestVectorSet vectorSet = result.ParsedObject;
            var casesWithKey = vectorSet.TestGroups.SelectMany(g => g.Tests.Where(t => ((TestCase)t).Key != null));
            Assert.IsNotEmpty(casesWithKey);
        }

        [Test]
        public void ShouldHaveTestsWithPlainTextFilled()
        {
            LegacyResponseFileParser subject = new LegacyResponseFileParser();
            string path = Path.Combine(_unitTestPath, "ECBVarKey128.rsp");
            var result = subject.Parse(path);
            Assume.That(result != null);
            TestVectorSet vectorSet = result.ParsedObject;
            var casesWithPlainText = vectorSet.TestGroups.SelectMany(g => g.Tests.Where(t => ((TestCase)t).PlainText != null));
            Assert.IsNotEmpty(casesWithPlainText);
        }

        [Test]
        public void ShouldHaveTestsWithCipherTextFilled()
        {
            LegacyResponseFileParser subject = new LegacyResponseFileParser();
            string path = Path.Combine(_unitTestPath, "ECBVarKey128.rsp");
            var result = subject.Parse(path);
            Assume.That(result != null);
            TestVectorSet vectorSet = result.ParsedObject;
            var casesWithCipherText = vectorSet.TestGroups.SelectMany(g => g.Tests.Where(t => ((TestCase)t).CipherText != null));
            Assert.IsNotEmpty(casesWithCipherText);
        }
    }
}
