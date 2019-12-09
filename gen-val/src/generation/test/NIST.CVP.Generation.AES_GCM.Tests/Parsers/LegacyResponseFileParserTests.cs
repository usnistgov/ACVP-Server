using System;
using System.IO;
using System.Linq;
using NIST.CVP.Generation.AES_GCM.v1_0;
using NIST.CVP.Generation.AES_GCM.v1_0.Parsers;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_GCM.Tests.Parsers
{
    [TestFixture, UnitTest]
    public class LegacyResponseFileParserTests
    {
        private string _unitTestPath;

        [OneTimeSetUp]
        public void Setup()
        {
            _unitTestPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\");
        }

        [Test]
        public void ShouldReturnErrorForNonExistentPath()
        {
            var subject = new LegacyResponseFileParser();
            var result = subject.Parse($@"C:\{Guid.NewGuid()}\testResults.json");
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void ShouldReturnErrorForNullOrEmptyPath(string path)
        {
            var subject = new LegacyResponseFileParser();
            var result = subject.Parse(path);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
        }

        [Test]
        public void ShouldParseValidFile()
        {
            var subject = new LegacyResponseFileParser();
            var path = Path.Combine(_unitTestPath, "gcmEncryptIntIV128.rsp");
            var result = subject.Parse(path);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldHaveProperNumberOfGroups()
        {
            var subject = new LegacyResponseFileParser();
            var path = Path.Combine(_unitTestPath, "gcmEncryptIntIV128.rsp");
            var result = subject.Parse(path);
            Assume.That(result != null);
            var vectorSet = result.ParsedObject;
            Assert.AreEqual(25, vectorSet.TestGroups.Count);
        }

        [Test]
        public void ShouldHaveTestsWithAADFilled()
        {
            var subject = new LegacyResponseFileParser();
            var path = Path.Combine(_unitTestPath, "gcmEncryptIntIV128.rsp");
            var result = subject.Parse(path);
            Assume.That(result != null);
            var vectorSet = result.ParsedObject;
            var casesWithAAD = vectorSet.TestGroups.SelectMany(g => g.Tests.Where(t => ((TestCase) t).AAD != null));
            Assert.IsNotEmpty(casesWithAAD);
        }
        [Test]
        public void ShouldHaveTestsWithKeyFilled()
        {
            var subject = new LegacyResponseFileParser();
            var path = Path.Combine(_unitTestPath, "gcmEncryptIntIV128.rsp");
            var result = subject.Parse(path);
            Assume.That(result != null);
            var vectorSet = result.ParsedObject;
            var casesWithKey = vectorSet.TestGroups.SelectMany(g => g.Tests.Where(t => ((TestCase)t).Key != null));
            Assert.IsNotEmpty(casesWithKey);
        }

        [Test]
        public void ShouldHaveTestsWithIVFilled()
        {
            var subject = new LegacyResponseFileParser();
            var path = Path.Combine(_unitTestPath, "gcmEncryptIntIV128.rsp");
            var result = subject.Parse(path);
            Assume.That(result != null);
            var vectorSet = result.ParsedObject;
            var casesWithIV = vectorSet.TestGroups.SelectMany(g => g.Tests.Where(t => ((TestCase)t).IV != null));
            Assert.IsNotEmpty(casesWithIV);
        }
        [Test]
        public void ShouldHaveTestsWithTagFilled()
        {
            var subject = new LegacyResponseFileParser();
            var path = Path.Combine(_unitTestPath, "gcmEncryptIntIV128.rsp");
            var result = subject.Parse(path);
            Assume.That(result != null);
            var vectorSet = result.ParsedObject;
            var casesWithTag = vectorSet.TestGroups.SelectMany(g => g.Tests.Where(t => ((TestCase)t).Tag != null));
            Assert.IsNotEmpty(casesWithTag);
        }

        [Test]
        public void ShouldHaveTestsWithPlainTextFilled()
        {
            var subject = new LegacyResponseFileParser();
            var path = Path.Combine(_unitTestPath, "gcmEncryptIntIV128.rsp");
            var result = subject.Parse(path);
            Assume.That(result != null);
            var vectorSet = result.ParsedObject;
            var casesWithPlainText = vectorSet.TestGroups.SelectMany(g => g.Tests.Where(t => ((TestCase)t).PlainText != null));
            Assert.IsNotEmpty(casesWithPlainText);
        }

        [Test]
        public void ShouldHaveTestsWithCipherTextFilled()
        {
            var subject = new LegacyResponseFileParser();
            var path = Path.Combine(_unitTestPath, "gcmEncryptIntIV128.rsp");
            var result = subject.Parse(path);
            Assume.That(result != null);
            var vectorSet = result.ParsedObject;
            var casesWithCipherText = vectorSet.TestGroups.SelectMany(g => g.Tests.Where(t => ((TestCase)t).CipherText != null));
            Assert.IsNotEmpty(casesWithCipherText);
        }

        [Test]
        public void ShouldParseValidDecryptFile()
        {
            var subject = new LegacyResponseFileParser();
            var path = Path.Combine(_unitTestPath, "gcmDecrypt256.rsp");
            var result = subject.Parse(path);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldParseValidDecryptFileWithFailureTest()
        {
            var subject = new LegacyResponseFileParser();
            var path = Path.Combine(_unitTestPath, "gcmDecrypt256.rsp");
            var result = subject.Parse(path);
            Assume.That(result != null);
            var vectorSet = result.ParsedObject;
            var casesWithFailureTests = vectorSet.TestGroups
                .SelectMany(g => g.Tests
                    .Where(t => t.TestPassed != null && !t.TestPassed.Value)
                );
            Assert.IsNotEmpty(casesWithFailureTests);
        }

        [Test]
        public void ShouldParseDecryptDirectionFromFileName()
        {
            var subject = new LegacyResponseFileParser();
            var path = Path.Combine(_unitTestPath, "gcmDecrypt256.rsp");
            var result = subject.Parse(path);
            Assume.That(result != null);
            var vectorSet = result.ParsedObject;
            Assert.IsTrue(vectorSet.TestGroups.All(g => ((TestGroup) g).Function == "decrypt"));

        }


        [Test]
        public void ShouldParseEncryptDirectionFromFileName()
        {
            var subject = new LegacyResponseFileParser();
            var path = Path.Combine(_unitTestPath, "gcmEncryptIntIV128.rsp");
            var result = subject.Parse(path);
            Assume.That(result != null);
            var vectorSet = result.ParsedObject;
            Assert.IsTrue(vectorSet.TestGroups.All(g => ((TestGroup)g).Function == "encrypt"));
        }

        [Test]
        public void ShouldParseInternalGenerationFromFileName()
        {
            var subject = new LegacyResponseFileParser();
            var path = Path.Combine(_unitTestPath, "gcmEncryptIntIV128.rsp");
            var result = subject.Parse(path);
            Assume.That(result != null);
            var vectorSet = result.ParsedObject;
            Assert.IsTrue(vectorSet.TestGroups.All(g => ((TestGroup)g).IvGeneration == "internal"));
        }

        [Test]
        public void ShouldParseExternalGenerationFromFileName()
        {
            var subject = new LegacyResponseFileParser();
            var path = Path.Combine(_unitTestPath, "gcmEncryptExtIV256.rsp");
            var result = subject.Parse(path);
            Assume.That(result != null);
            var vectorSet = result.ParsedObject;
            Assert.IsTrue(vectorSet.TestGroups.All(g => ((TestGroup)g).IvGeneration == "external"));
        }
    }
}
