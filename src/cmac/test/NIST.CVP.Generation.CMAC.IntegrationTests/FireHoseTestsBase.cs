using NIST.CVP.Crypto.CMAC;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Generation.CMAC.v1_0.Parsers;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.CMAC.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public abstract class FireHoseTestsBase<TLegacyResponseFileParser>
        where TLegacyResponseFileParser : LegacyResponseFileParserBase, new()
    {
        string _testPath;
        protected abstract string FolderName { get; }

        protected readonly CmacFactory _subject = new CmacFactory(new BlockCipherEngineFactory(), new ModeBlockCipherFactory());

        [SetUp]
        public void Setup()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), $@"..\..\TestFiles\LegacyParserFiles\{FolderName}");
        }
 
        [Test]
        protected void ShouldRunThroughAllTestFilesAndValidate()
        {
            var parser = new TLegacyResponseFileParser();
            var parsedFiles = parser.Parse(_testPath);
            
            if (!parsedFiles.Success)
            {
                Assert.Fail("Failed parsing test files");
            }

            if (parsedFiles.ParsedObject.TestGroups.Count == 0)
            {
                Assert.Fail("No TestGroups were parsed.");
            }
            var testVector = parsedFiles.ParsedObject;
            

            int count = 0;
            int passes = 0;
            int fails = 0;
            int failureTests = 0;
            
            if (testVector.TestGroups.Count == 0)
            {
                Assert.Fail("No TestGroups were parsed.");
            }

            foreach (var testGroup in testVector.TestGroups)
            {
                foreach (var testCase in testGroup.Tests)
                {
                    count++;

                    if (testGroup.Function.ToLower() == "gen")
                    {
                        var result = _subject.GetCmacInstance(testGroup.CmacType)
                            .Generate(testCase.Key, testCase.Message, testGroup.MacLength);

                        if (!result.Success)
                        {
                            fails++;
                            continue;
                        }

                        if (testCase.Mac.ToHex() == result.Mac.ToHex())
                        {
                            passes++;
                        }
                        else
                        {
                            fails++;
                        }

                        Assert.AreEqual(
                            testCase.Mac.ToHex(),
                            result.Mac.ToHex(),
                            $"Failed on count {count} expected CT {testCase.Mac.ToHex()}, got {result.Mac.ToHex()}"
                        );
                        continue;
                    }

                    if (testGroup.Function.ToLower() == "ver")
                    {
                        var result = _subject.GetCmacInstance(testGroup.CmacType)
                            .Verify(testCase.Key, testCase.Message, testCase.Mac);

                        if (testCase.TestPassed != null && !testCase.TestPassed.Value)
                        {
                            failureTests++;
                            if (result.Success)
                            {
                                fails++;
                            }
                            else
                            {
                                passes++;
                            }
                        }
                        else
                        {
                            if (result.Success)
                            {
                                passes++;
                            }
                            else
                            {
                                fails++;
                            }
                        }
                        continue;
                    }

                    Assert.Fail($"{testGroup.Function} did not meet expected function values");
                }
            }

            if (fails > 0)
                Assert.Fail("Unexpected failures were encountered.");

            //Assert.Fail($"Passes {passes}, fails {fails}, count {count}.  Failure tests {failureTests}");
        }
       
    }
}
