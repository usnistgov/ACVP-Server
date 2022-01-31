using System;
using System.IO;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Generation.AES_CFB1.v1_0;
using NIST.CVP.ACVTS.Libraries.Generation.AES_CFB1.v1_0.Parsers;
using NIST.CVP.ACVTS.Tests.Core;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.AES.CFB1.Parsers
{
    [TestFixture, UnitTest]
    public class LegacyResponseFileParserTests
    {

        private string _unitTestPath;
        private int _expectedGroups = 6;
        #region File
        private string _testFileContents = @"
# CAVS 20.2
# Config info for test
# AESVS MMT test data for CFB1
# State : Encrypt and Decrypt
# Key Length : 128
# Generated on Wed Feb  1 11:10:18 2017

[ENCRYPT]

COUNT = 0
KEY = 6b99530b1e61999d795bc21d4a329bb1
IV = 2e54216f1468d3ced9af7aa62a744340
PLAINTEXT = 1
CIPHERTEXT = 1

COUNT = 1
KEY = 2695b44439192d099f0a31b89f24dc0f
IV = 5086c0ed84593ed006919af33d4c8902
PLAINTEXT = 11
CIPHERTEXT = 11

COUNT = 2
KEY = a258b0ac5e4aac83b5eb1798b957b07a
IV = 89f3f98a9dff3bd2d65c448f57f46e84
PLAINTEXT = 010
CIPHERTEXT = 001

COUNT = 3
KEY = 29bfd927d99fad931f708d39a1d5e4e4
IV = 194e80ea5af497cd7e600898ff783204
PLAINTEXT = 0001
CIPHERTEXT = 0000

COUNT = 4
KEY = e140e98ef14487f9219ef770ab7652f6
IV = 8b69a3e7f724b621282ef87cfc43452b
PLAINTEXT = 00000
CIPHERTEXT = 01001

COUNT = 5
KEY = 7f612e1b461b0aa58fd17cd1c253e413
IV = 44301267ab8b95674b03629c1d7e5ea5
PLAINTEXT = 001111
CIPHERTEXT = 100011

COUNT = 6
KEY = 59d22b3d64cd29bdb4aa6ca916514dc2
IV = a2de14277eb97664d4c561b3fd4b0e32
PLAINTEXT = 1110110
CIPHERTEXT = 0010011

COUNT = 7
KEY = b07a1be66b5a84d56b3a852e8c948159
IV = 148ec9127dd4b64e01e10448a96d181f
PLAINTEXT = 01100110
CIPHERTEXT = 10111011

COUNT = 8
KEY = 4867b9675bb18d15186ec1763a769d45
IV = 6f2da02d066b1e52fe8fce31d6ed4263
PLAINTEXT = 110111110
CIPHERTEXT = 000001111

COUNT = 9
KEY = 7df9fff1c14de3e251aecd3a4328a907
IV = 3903d581031b6b60c0162185bc223fa9
PLAINTEXT = 0100100001
CIPHERTEXT = 0100011111

[DECRYPT]

COUNT = 0
KEY = b932af6fbaa5328ea456125848a0c9ad
IV = 03056d9fb796f2a040936e13778612f2
CIPHERTEXT = 0
PLAINTEXT = 0

COUNT = 1
KEY = 83185129c67311b6fc765639b7bb63c0
IV = 84324d79ab6adc655d295ee9f8263725
CIPHERTEXT = 10
PLAINTEXT = 01

COUNT = 2
KEY = 7e40582ad52047ea2c95b95eb2a4f0d0
IV = 42eea67c6fe38cb6337b59cbc3a39926
CIPHERTEXT = 101
PLAINTEXT = 101

COUNT = 3
KEY = 5bf4bc5c803810399e0193605e61d6a8
IV = 9abd1387b8aca6e2a52d921b4994a15b
CIPHERTEXT = 1001
PLAINTEXT = 1111

COUNT = 4
KEY = 806abc051cf4443a59eb92f47ad91aac
IV = caee4e3a00fc49115e785b8d131c60e9
CIPHERTEXT = 00001
PLAINTEXT = 00001

COUNT = 5
KEY = 32f9ddf70c91e99ef57b77463f1489bf
IV = 3acaabc7e620dfcb3c388936030fc67f
CIPHERTEXT = 000110
PLAINTEXT = 000000

COUNT = 6
KEY = 97827f382e5b54de14634fee17fbe071
IV = 21c2a78d47447d3419be821007acd021
CIPHERTEXT = 1000101
PLAINTEXT = 0100000

COUNT = 7
KEY = 9847624ad6998aba0e9bca68fc276615
IV = 5fec980df264de735a9d1b3ac4620518
CIPHERTEXT = 00000100
PLAINTEXT = 11011101

COUNT = 8
KEY = 4bf665f422bb74ec10b290d8c84eacf1
IV = 27be25e090ca6b0b56e032dab893cd28
CIPHERTEXT = 010000001
PLAINTEXT = 001010110

COUNT = 9
KEY = 2a5e39f2de044223a0fcde0327331602
IV = 5e343033f92a3c82efdf9a35ac5e3657
CIPHERTEXT = 1001110000
PLAINTEXT = 0000011011
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
            File.WriteAllText(Path.Combine(_unitTestPath, "file1.fax"), _testFileContents); // 2 groups, 18 tests 
            File.WriteAllText(Path.Combine(_unitTestPath, "file2.fax"), _testFileContents); // + 2 groups, 18 tests
            File.WriteAllText(Path.Combine(_unitTestPath, "fileThatShouldntBeParsed.dat"), _testFileContents); // + 0 (shouldn't be included)
            File.WriteAllText(Path.Combine(_unitTestPath, "MCT.fax"), _testFileContents); // + 2 groups, 18 tests
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
            var result = subject.Parse(Path.Combine(_unitTestPath, $"{Guid.NewGuid()}.rsp"));
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
            Assert.That(result != null);
            var vectorSet = result.ParsedObject;
            Assert.AreEqual(6, vectorSet.TestGroups.Count);
        }

        [Test]
        public void ShouldHaveTestsWithKeyFilled()
        {
            var subject = new LegacyResponseFileParser();
            var result = subject.Parse(_unitTestPath);
            Assert.That(result != null);
            var vectorSet = result.ParsedObject;
            var casesWithKey = vectorSet.TestGroups.SelectMany(g => g.Tests.Where(t => ((TestCase)t).Key != null));
            Assert.IsNotEmpty(casesWithKey);
        }

        [Test]
        public void ShouldHaveTestsWithIVFilled()
        {
            var subject = new LegacyResponseFileParser();
            var result = subject.Parse(_unitTestPath);
            Assert.That(result != null);
            var vectorSet = result.ParsedObject;
            var casesWithIV = vectorSet.TestGroups.SelectMany(g => g.Tests.Where(t => ((TestCase)t).IV != null));
            Assert.IsNotEmpty(casesWithIV);
        }

        [Test]
        public void ShouldHaveTestsWithPlainTextFilled()
        {
            var subject = new LegacyResponseFileParser();
            var result = subject.Parse(_unitTestPath);
            Assert.That(result != null);
            var vectorSet = result.ParsedObject;
            var casesWithPlainText = vectorSet.TestGroups.SelectMany(g => g.Tests.Where(t => ((TestCase)t).PlainText != null));
            Assert.IsNotEmpty(casesWithPlainText);
        }

        [Test]
        public void ShouldHaveTestsWithCipherTextFilled()
        {
            var subject = new LegacyResponseFileParser();
            var result = subject.Parse(_unitTestPath);
            Assert.That(result != null);
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
            Assert.That(result != null);
            var vectorSet = result.ParsedObject;
            Assert.That(vectorSet.TestGroups.Count() == _expectedGroups);
            var decryptCount = vectorSet.TestGroups.Count(g => ((TestGroup)g).Function.ToLower() == "decrypt");
            Assert.AreEqual(3, decryptCount, decryptCount.ToString());
        }

        [Test]
        public void ShouldParseEncryptFromFileContents()
        {
            var subject = new LegacyResponseFileParser();
            var result = subject.Parse(_unitTestPath);
            Assert.That(result != null);
            var vectorSet = result.ParsedObject;
            Assert.That(vectorSet.TestGroups.Count() == _expectedGroups);
            var encryptCount = vectorSet.TestGroups.Count(g => ((TestGroup)g).Function.ToLower() == "encrypt");
            Assert.AreEqual(3, encryptCount, encryptCount.ToString());

        }
    }
}
