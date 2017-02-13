using System;
using System.IO;
using System.Linq;
using NIST.CVP.Generation.AES_CFB8.Parsers;
using NIST.CVP.Tests.Core;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_CFB8.Tests.Parsers
{
    [TestFixture]
    public class LegacyResponseFileParserTests
    {

        private string _unitTestPath;
        private int _groupsPerFile = 2;
        private int _TestsPerGroup = 9;
        #region File
        private string _testFIleContents = @"
# CAVS 20.2
# Config info for test
# AESVS MMT test data for CFB8
# State : Encrypt and Decrypt
# Key Length : 128
# Generated on Fri Feb 10 14:31:55 2017

[ENCRYPT]

COUNT = 0
KEY = 3772dac842f89bce8d3aa28f0cb58dfb
IV = e7a0a537137751db66fb882ec80bfc27
PLAINTEXT = 7c
CIPHERTEXT = 6d

COUNT = 1
KEY = e22d39c1d1988f3554b973f9f739f4ac
IV = f69867ca5fd291fdf2db768fb1775615
PLAINTEXT = 6fcf
CIPHERTEXT = a1fd

COUNT = 2
KEY = e59ef7d52c3ff18c5cf638b32dea6552
IV = 3d5fe9afa0b2b62bb9f96cad8db6d1ed
PLAINTEXT = 7ed9c7
CIPHERTEXT = 50d78a

COUNT = 3
KEY = bd91aff00a15190fdb6335f1d4d7f983
IV = fd20e247872b055b1c3ba5415515cbf3
PLAINTEXT = 7414bf4b
CIPHERTEXT = 50f9a22d

COUNT = 4
KEY = 3ab0604b06112b61a0eed1f3a00f5017
IV = 281d700ee30b8d46d0c711d50e84b0b2
PLAINTEXT = 56b4384500
CIPHERTEXT = c925b540cd

COUNT = 5
KEY = 2b1a380d2d62b6d64fc37d018927606c
IV = a9239594c9027b91ff1659e960f5fa87
PLAINTEXT = 07b1b49c40aa
CIPHERTEXT = 23df7d409c21

COUNT = 6
KEY = c6c6cd58c778dbfbac994691ad949bfb
IV = d61a361800122be3e3d8cc6a51c7948c
PLAINTEXT = e1467ae30a1e36
CIPHERTEXT = 289e2f40aba2d6

COUNT = 7
KEY = eb0a89a9e21abb266eaf92242f784eef
IV = e5ac710126e804b8f6d947eaad0ce6a4
PLAINTEXT = 2c521df5f6cd662b
CIPHERTEXT = 4fae9bd41f603a4b

COUNT = 8
KEY = 1ad295387f4a78f263a2a11f22d7edb9
IV = c20db61032ab57fc423f8ad3ae6affd4
PLAINTEXT = 097690ce60f9aec4d7
CIPHERTEXT = a23076e91e85b82802

COUNT = 9
KEY = 811747ce1fea043a39c342625ec52b1a
IV = e09c8d3be53be9bdd63f99b70f630149
PLAINTEXT = eb0a89a9e21abb266eaf
CIPHERTEXT = 751c5e72da27a3a616f4

[DECRYPT]

COUNT = 0
KEY = 8cc6fbe10e9d9ccc202be941f0742683
IV = 99234417a7cf01dba91d5565bf046b00
CIPHERTEXT = 81
PLAINTEXT = e4

COUNT = 1
KEY = a153ba03df9544b12fd91f6f4bc0f589
IV = e26feb7f61cf7425e326188719b32488
CIPHERTEXT = db35
PLAINTEXT = 938b

COUNT = 2
KEY = 00eb67ef5168d916a3635d6c3623ea9e
IV = d7a24ed48fa8f6ed52777bc5d8faeeb8
CIPHERTEXT = 6a52a4
PLAINTEXT = ce5c84

COUNT = 3
KEY = af3ff49892616cd628f4f5b171c8b30f
IV = 5cf561699b95a56c601ec423458f5020
CIPHERTEXT = 68ddf37c
PLAINTEXT = 06fc4e46

COUNT = 4
KEY = 9b8980081728abafeb8b22a6346c8992
IV = 175fde0f61ec393393badb69e0481743
CIPHERTEXT = aa2e16b1da
PLAINTEXT = 38a8822b7d

COUNT = 5
KEY = 8106d96810cc7320a32a51d5d5e08d4d
IV = 9d9aa3bb4ac8bfb1c57f62b14c955963
CIPHERTEXT = 3ce358ead0fe
PLAINTEXT = da3246c68302

COUNT = 6
KEY = 694622a19b38f37e584c182851bcfa33
IV = 687c59277d6b43845940c440a70f712b
CIPHERTEXT = aaaca08a6d0dc4
PLAINTEXT = bfd97551c7e4c3

COUNT = 7
KEY = b6199cff449ad8517ca5d826b702d91b
IV = 707c84d5d27c504b269dd0654b16ebbf
CIPHERTEXT = 491538714fc90b63
PLAINTEXT = 6e6f32ceb9804e58

COUNT = 8
KEY = 5a61ce6f844e0318e93b26344b3323c8
IV = e15102e1a3ad4e20bf142d85c4a7ced8
CIPHERTEXT = 400c7ed925dfc407d6
PLAINTEXT = a5a4383e3a2afca800

COUNT = 9
KEY = 81d78365438bbb00e7807546f6ee99d1
IV = d27d153413f24ffba2db18589ee6319c
CIPHERTEXT = 643644bbe279795c7c73
PLAINTEXT = 4f34dba6219cc94d86a8
";
        #endregion File

        [OneTimeSetUp]
        public void Setup()
        {
            _unitTestPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\temp_LegacyFileParser");
            if (Directory.Exists(_unitTestPath))
            {
                Directory.Delete(_unitTestPath, true);
            }
            Directory.CreateDirectory(_unitTestPath);
            File.WriteAllText($@"{_unitTestPath}\file1.fax", _testFIleContents); // 2 groups, 18 tests 
            File.WriteAllText($@"{_unitTestPath}\file2.fax", _testFIleContents); // + 2 groups, 18 tests
            File.WriteAllText($@"{_unitTestPath}\fileThatShouldntBeParsed.dat", _testFIleContents); // + 0 (shouldn't be included)
            File.WriteAllText($@"{_unitTestPath}\MCT.fax", _testFIleContents); // + 2 groups, 18 tests
            // Total groups = 6, total tests = 54
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            Directory.Delete(_unitTestPath, true);
        }

        [Test]
        public void ShouldReturnErrorForNonExistentPath()
        {
            var subject = new LegacyResponseFileParser();
            var result = subject.Parse($@"{_unitTestPath}\{Guid.NewGuid()}.rsp");
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
            var result = subject.Parse(_unitTestPath);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldHaveProperNumberOfGroups()
        {
            var subject = new LegacyResponseFileParser();
            var result = subject.Parse(_unitTestPath);
            Assume.That(result != null);
            var vectorSet = result.ParsedObject;
            Assert.AreEqual(6, vectorSet.TestGroups.Count);
        }
        
        [Test]
        public void ShouldHaveTestsWithKeyFilled()
        {
            var subject = new LegacyResponseFileParser();
            var result = subject.Parse(_unitTestPath);
            Assume.That(result != null);
            var vectorSet = result.ParsedObject;
            var casesWithKey = vectorSet.TestGroups.SelectMany(g => g.Tests.Where(t => ((TestCase)t).Key != null));
            Assert.IsNotEmpty(casesWithKey);
        }

        [Test]
        public void ShouldHaveTestsWithIVFilled()
        {
            var subject = new LegacyResponseFileParser();
            var result = subject.Parse(_unitTestPath);
            Assume.That(result != null);
            var vectorSet = result.ParsedObject;
            var casesWithIV = vectorSet.TestGroups.SelectMany(g => g.Tests.Where(t => ((TestCase)t).IV != null));
            Assert.IsNotEmpty(casesWithIV);
        }
        
        [Test]
        public void ShouldHaveTestsWithPlainTextFilled()
        {
            var subject = new LegacyResponseFileParser();
            var result = subject.Parse(_unitTestPath);
            Assume.That(result != null);
            var vectorSet = result.ParsedObject;
            var casesWithPlainText = vectorSet.TestGroups.SelectMany(g => g.Tests.Where(t => ((TestCase)t).PlainText != null));
            Assert.IsNotEmpty(casesWithPlainText);
        }

        [Test]
        public void ShouldHaveTestsWithCipherTextFilled()
        {
            var subject = new LegacyResponseFileParser();
            var result = subject.Parse(_unitTestPath);
            Assume.That(result != null);
            var vectorSet = result.ParsedObject;
            var casesWithCipherText = vectorSet.TestGroups.SelectMany(g => g.Tests.Where(t => ((TestCase)t).CipherText != null));
            Assert.IsNotEmpty(casesWithCipherText);
        }

        [Test]
        public void ShouldParseValidDecryptFile()
        {
            var subject = new LegacyResponseFileParser();
            var result = subject.Parse(_unitTestPath);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }
        
        [Test]
        public void ShouldParseDecryptFromFileContents()
        {
            var subject = new LegacyResponseFileParser();
            var result = subject.Parse(_unitTestPath);
            Assume.That(result != null);
            var vectorSet = result.ParsedObject;
            Assume.That(vectorSet.TestGroups.Count() == 6);
            var decryptCount = vectorSet.TestGroups.Count(g => ((TestGroup) g).Function.ToLower() == "decrypt");
            Assert.AreEqual(3, decryptCount, decryptCount.ToString());
        }

        [Test]
        public void ShouldParseEncryptFromFileContents()
        {
            var subject = new LegacyResponseFileParser();
            var result = subject.Parse(_unitTestPath);
            Assume.That(result != null);
            var vectorSet = result.ParsedObject;
            Assume.That(vectorSet.TestGroups.Count() == 6);
            var encryptCount = vectorSet.TestGroups.Count(g => ((TestGroup)g).Function.ToLower() == "encrypt");
            Assert.AreEqual(3, encryptCount, encryptCount.ToString());

        }
    }
}
