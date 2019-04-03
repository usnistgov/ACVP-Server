using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using NIST.CVP.Common;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.RSA_DPComponent.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm => "RSA";
        public override string Mode => "DecryptionPrimitive";

        public override AlgoMode AlgoMode => AlgoMode.RSA_DecryptionPrimitive_v1_0;

        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();
		public override IRegisterInjections RegistrationsCrypto => new Crypto.RegisterInjections();

        protected override string GetTestFileFewTestCases(string folderName)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                IsSample = true,
            };

            return CreateRegistration(folderName, p);
        }

        protected override string GetTestFileLotsOfTestCases(string folderName)
        {
            return GetTestFileFewTestCases(folderName);
        }

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            // If TC has a resultsArray, change some of the elements
            if (testCase.resultsArray != null)
            {
                // If TC has a result, change it
                if (testCase.resultsArray[0].testPassed != null)
                {
                    testCase.resultsArray[0].testPassed = !((bool)testCase.resultsArray[0].testPassed);
                }

                // If TC has a pt, change it
                if (testCase.resultsArray[0].plainText != null)
                {
                    var bs = new BitString(testCase.resultsArray[0].plainText.ToString());
                    bs = rand.GetDifferentBitStringOfSameSize(bs);

                    // Can't get something "different" of empty bitstring of the same length
                    if (bs == null)
                    {
                        bs = new BitString("01");
                    }

                    testCase.resultsArray[0].plainText = bs.ToHex();
                }
            }
        }
    }
}