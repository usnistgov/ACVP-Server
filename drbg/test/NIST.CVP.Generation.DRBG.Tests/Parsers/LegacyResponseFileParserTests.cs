using System;
using System.IO;
using System.Linq;
using NIST.CVP.Generation.DRBG.Parsers;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DRBG.Tests.Parsers
{
    [TestFixture, UnitTest]
    public class LegacyResponseFileParserTests
    {

        private string _unitTestPath;
        private int _expectedGroups = 8;
        private const string _FILE_EXTENSION = ".fax";
        #region File
        private string _testFIleContents = @"
[AES-128 use df]
[PredictionResistance = True]
[EntropyInputLen = 128]
[NonceLen = 64]
[PersonalizationStringLen = 0]
[AdditionalInputLen = 0]
[ReturnedBitsLen = 512]

COUNT = 0
EntropyInput = 10268ba2f388bf5e9bf37ed2df890d2c
Nonce = 94b4af493ebe7d70
PersonalizationString = 
AdditionalInput = 
EntropyInputPR = 26c0ac3667dc04bd4601861827161a1d
AdditionalInput = 
EntropyInputPR = 9da5b6b2fb19a31415c58b0d86aeb5b9
ReturnedBits = 900234ab75c7edbf1c8960407d75a5fef92a8460cacfa0454f7217b482ac6d7a73f962e8fbf680537ee4a3e94567d5c441158d7b8f601325d9a73212296c382c

COUNT = 1
EntropyInput = dc88d77a0281f5d804f35c72502ef730
Nonce = 9c1d850f631b3508
PersonalizationString = 
AdditionalInput = 
EntropyInputPR = 03bb411b907231cf8ae18c70cfe2b1fc
AdditionalInput = 
EntropyInputPR = 4e79b77c986ab1b14ea764f3054795a0
ReturnedBits = ba8f51c6a124dd4986a3875b069cdbe18250ad5c78c5c32aa07074e690c575b938fa62e5352350eb4c21af698d3d03f1059da62e6eaca7f9a5b23c12649b59b1

[AES-128 use df]
[PredictionResistance = False]
[EntropyInputLen = 128]
[NonceLen = 64]
[PersonalizationStringLen = 128]
[AdditionalInputLen = 0]
[ReturnedBitsLen = 512]

COUNT = 0
EntropyInput = df824aaa8d2f83ee347e7d0024538258
Nonce = 78c177601ee46e12
PersonalizationString = 41dbb213dbd9682976e622df96e9dad2
EntropyInputReseed = c2e70991b209ba9972e4dc99af86a728
AdditionalInputReseed = 
AdditionalInput = 
AdditionalInput = 
ReturnedBits = 6781d9f25ef89a5c353de4f02707179d6422bc3aa2db8c0cd1eba64eacbe4d77101ef0608728f03e78d62de715b9861e2bc9828280b2c51217e32845d1e84d95

COUNT = 1
EntropyInput = 081d3be94cedd21e7adedae63821ad74
Nonce = 2c6c701619eb3f6c
PersonalizationString = 4049bf29089095da815482f5dcaa4ecb
EntropyInputReseed = 2a58501d4f81217821f15cd12fe549ac
AdditionalInputReseed = 
AdditionalInput = 
AdditionalInput = 
ReturnedBits = 57508cc4dc2ba44809b6824ebf5150b6d2f9fbd8404a9faffdac2844d68a60384ea4f91241851e0458f9d34d88954227dbf4dea180790115c1ef5824de2c8073

[AES-128 no df]
[PredictionResistance = False]
[EntropyInputLen = 128]
[NonceLen = 64]
[PersonalizationStringLen = 0]
[AdditionalInputLen = 0]
[ReturnedBitsLen = 512]

COUNT = 0
EntropyInput = e4c5c3f019d79a7b5e84752f485784b6
Nonce = 3fdfac127c2cfb58
PersonalizationString = 
AdditionalInput = 
AdditionalInput = 
ReturnedBits = f12155e2f6ba503fe3afc746d738e06bd9dad32da572afc35ad0d724073792279044310d80111ece5c442f65e478a834545350137a2430193e2242c4e38b261a

COUNT = 1
EntropyInput = 6608576ac066f69498874da07d01eeb7
Nonce = 583171ec5854743f
PersonalizationString = 
AdditionalInput = 
AdditionalInput = 
ReturnedBits = daccbf27cb51dda7415f4c793b5d7f2221d88abbe9dbee64f19906f7a4de68db34542182a6b7c9881720c81f099966727e348a82012f5e3229b0c40527aad414

[AES-128 use df]
[PredictionResistance = True]
[EntropyInputLen = 128]
[NonceLen = 64]
[PersonalizationStringLen = 128]
[AdditionalInputLen = 128]
[ReturnedBitsLen = 512]

COUNT = 0
EntropyInput = 2db98dc9facda5091648d2c98d31e60b
Nonce = e58a44792a914dbf
PersonalizationString = 2e00d270b935494caa98bdb5d4322f42
AdditionalInput = ca37ba3574bd9d9f3c799604fce19950
EntropyInputPR = 48954be19f1b9d4c442e302af62de176
AdditionalInput = dfaaa596f1985b6434c0683f52a3a760
EntropyInputPR = 8ece8f8d181cb6c9b1eec143d2afe57e
ReturnedBits = 653408360562b6ad0a66046e018adcb70dd444aedc17e2a4a62456b5709a780d685cdf77257d4956c50352b5b908924c8f3954d83e9a2c95dfecd495e8e39958

COUNT = 1
EntropyInput = d1d0c0a2a579408cf03ad000eb3749cf
Nonce = d5605599aa9dc75b
PersonalizationString = beb0d682f2ee4a0274a7b4bc0dd37100
AdditionalInput = c7e00aa4a8598c3bf2c3e6128d81d8b8
EntropyInputPR = 93d461f739e59bbc424f627e0705e958
AdditionalInput = 020ade3f12e062ec3f57758e407cbf0f
EntropyInputPR = 2f97cf0c74528bdb54a0dbddf2a1c3ff
ReturnedBits = 33e28f90c4a16799b18710a2467b8f56a5439a3263445ccc75885293fed1c754638660fe1099068c12bd494365a53dcb0cbadcb87ad942a0a2585e3852a6e24d

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
            File.WriteAllText($@"{_unitTestPath}\file1{_FILE_EXTENSION}", _testFIleContents); // 4 groups, 8 tests 
            File.WriteAllText($@"{_unitTestPath}\file2{_FILE_EXTENSION}", _testFIleContents); // + 4 groups, 8 tests
            File.WriteAllText($@"{_unitTestPath}\fileThatShouldntBeParsed.dat", _testFIleContents); // + 0 (shouldn't be included)
            // Total groups = 8, total tests = 16
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
            var result = subject.Parse($@"{_unitTestPath}\{Guid.NewGuid()}{_FILE_EXTENSION}");
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
            Assert.AreEqual(_expectedGroups, vectorSet.TestGroups.Count);
        }

        [Test]
        public void ShouldHaveTestsWithEntropyInputFilled()
        {
            var subject = new LegacyResponseFileParser();
            var result = subject.Parse(_unitTestPath);
            Assume.That(result != null);
            var vectorSet = result.ParsedObject;
            var cases = vectorSet.TestGroups.SelectMany(g => g.Tests.Where(t => ((TestCase)t).EntropyInput != null));
            Assert.IsNotEmpty(cases);
        }

        [Test]
        public void ShouldHaveTestsWithNonceFilled()
        {
            var subject = new LegacyResponseFileParser();
            var result = subject.Parse(_unitTestPath);
            Assume.That(result != null);
            var vectorSet = result.ParsedObject;
            var cases = vectorSet.TestGroups.SelectMany(g => g.Tests.Where(t => ((TestCase)t).Nonce != null));
            Assert.IsNotEmpty(cases);
        }

        [Test]
        public void ShouldHaveTestsWithPersoStringFilled()
        {
            var subject = new LegacyResponseFileParser();
            var result = subject.Parse(_unitTestPath);
            Assume.That(result != null);
            var vectorSet = result.ParsedObject;
            var cases = vectorSet.TestGroups.SelectMany(g => g.Tests.Where(t => ((TestCase)t).PersoString != null));
            Assert.IsNotEmpty(cases);
        }
        
        [Test]
        public void ShouldParsePredictionResistanceFromFileContents()
        {
            var subject = new LegacyResponseFileParser();
            var result = subject.Parse(_unitTestPath);
            Assume.That(result != null);
            var vectorSet = result.ParsedObject;
            Assume.That(vectorSet.TestGroups.Count() == _expectedGroups);
            var decryptCount = vectorSet.TestGroups.Count(g => ((TestGroup)g).PredResistance);
            Assert.AreEqual(4, decryptCount, decryptCount.ToString());
        }

        [Test]
        public void ShouldParseDerivationFromFileContents()
        {
            var subject = new LegacyResponseFileParser();
            var result = subject.Parse(_unitTestPath);
            Assume.That(result != null);
            var vectorSet = result.ParsedObject;
            Assume.That(vectorSet.TestGroups.Count() == _expectedGroups);
            var encryptCount = vectorSet.TestGroups.Count(g => ((TestGroup)g).DerFunc);
            Assert.AreEqual(6, encryptCount, encryptCount.ToString());
        }
    }
}
