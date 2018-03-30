﻿using Autofac;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.SNMP.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm => "kdf-components";
        public override string Mode => "snmp";

        public override AlgoMode AlgoMode => AlgoMode.KDFComponents_SNMP;

        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            if (testCase.sharedKey != null)
            {
                var bs = new BitString(testCase.sharedKey.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                testCase.sharedKey = bs.ToHex();
            }
        }

        protected override string GetTestFileFewTestCases(string folderName)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                EngineId = new [] {"12345678912345678900", "abcdef0123456789abcdef1234567890"},
                PasswordLength = new MathDomain().AddSegment(new ValueDomainSegment(128)).AddSegment(new ValueDomainSegment(1024)),
                IsSample = true
            };

            return CreateRegistration(folderName, p);
        }

        protected override string GetTestFileLotsOfTestCases(string folderName)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                EngineId = new [] {"12345678912345678900", "abcdef0123456789abcdef1234567890"},
                PasswordLength = new MathDomain().AddSegment(new ValueDomainSegment(ParameterValidator.PASSWORD_MINIMUM_LENGTH)).AddSegment(new ValueDomainSegment(ParameterValidator.PASSWORD_MAXIMUM_LENGTH)),
                IsSample = false
            };

            return CreateRegistration(folderName, p);
        }
    }
}
