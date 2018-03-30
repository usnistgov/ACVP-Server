using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Generation.GenValApp.Helpers;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_CFB1.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm { get; } = "AES";
        public override string Mode { get; } = "CFB1";

        public override AlgoMode AlgoMode => AlgoMode.AES_CFB1;

        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            // If TC is intended to be a failure test, change it
            if (testCase.decryptFail != null)
            {
                testCase.decryptFail = false;
            }

            // If TC has a cipherText, change it
            if (testCase.cipherText != null)
            {
                string text = testCase.cipherText.ToString();
                BitOrientedBitString bs = ModifyText(ref text);
                testCase.cipherText = new string(bs.ToString().Replace(" ", string.Empty).ToArray());
            }

            // If TC has a plainText, change it
            if (testCase.plainText != null)
            {
                string text = testCase.plainText.ToString();

                BitOrientedBitString bs = ModifyText(ref text);
                testCase.plainText = new string(bs.ToString().Replace(" ", string.Empty).ToArray());
            }

            // If TC has a resultsArray, change some of the elements
            if (testCase.resultsArray != null)
            {
                BitString bsIV = new BitString(testCase.resultsArray[0].iv.ToString());
                bsIV = rand.GetDifferentBitStringOfSameSize(bsIV);
                testCase.resultsArray[0].iv = bsIV.ToHex();

                BitString bsKey = new BitString(testCase.resultsArray[0].key.ToString());
                bsKey = rand.GetDifferentBitStringOfSameSize(bsKey);
                testCase.resultsArray[0].key = bsKey.ToHex();

                string plainText = testCase.resultsArray[0].plainText.ToString();
                BitOrientedBitString bsPlainText = ModifyText(ref plainText);
                testCase.resultsArray[0].plainText = new string(bsPlainText.ToString().Replace(" ", string.Empty).ToArray());

                string cipherText = testCase.resultsArray[0].cipherText.ToString();
                BitOrientedBitString bsCipherText = ModifyText(ref cipherText);
                testCase.resultsArray[0].cipherText = new string(bsCipherText.ToString().Replace(" ", string.Empty).ToArray());
            }
        }
        
        private static BitOrientedBitString ModifyText(ref string text)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var ch in text)
            {
                if (ch == '0')
                {
                    sb.Append('1');
                }
                else
                {
                    sb.Append('0');
                }
            }
            text = sb.ToString();
            var bs = BitOrientedBitString.GetBitStringEachCharacterOfInputIsBit(text);
            return bs;
        }
        
        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            Parameters p = new Parameters()
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Direction = new string[] { "encrypt", "decrypt" },
                KeyLen = new int[] { ParameterValidator.VALID_KEY_SIZES.First() },
                IsSample = true
            };

            return CreateRegistration(targetFolder, p);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            Parameters p = new Parameters()
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Direction = ParameterValidator.VALID_DIRECTIONS,
                KeyLen = ParameterValidator.VALID_KEY_SIZES,
                IsSample = false
            };

            return CreateRegistration(targetFolder, p);
        }

        /// <summary>
        /// Can be used to only generate MMT groups for the genval tests
        /// </summary>
        public class FakeTestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters>
        {
            public IEnumerable<ITestGroupGenerator<Parameters>> GetTestGroupGenerators()
            {
                return new List<ITestGroupGenerator<Parameters>>()
                {
                    new TestGroupGeneratorMultiBlockMessage()
                };
            }
        }

        protected override void OverrideRegisteredDependencies(ContainerBuilder builder)
        {
            base.OverrideRegisteredDependencies(builder);

            builder.RegisterType<FakeTestGroupGeneratorFactory>().AsImplementedInterfaces();
        }
    }
}
