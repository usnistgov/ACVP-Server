﻿using Autofac;
using NIST.CVP.Common;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Generation.KAS.EccComponent;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KAS.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTestsEccComponent : GenValTestsSingleRunnerBase
    {
        private readonly Random800_90 _random = new Random800_90();

        public override string Algorithm => "KAS-ECC";

        public override string Mode => "CDH-Component";

        public override AlgoMode AlgoMode => AlgoMode.KAS_EccComponent;

        public override IRegisterInjections RegistrationsCrypto => new Crypto.RegisterInjections();
        public override IRegisterInjections RegistrationsGenVal => new EccComponent.RegisterInjections();

        protected override string GetTestFileFewTestCases(string folderName)
        {
            EccComponent.Parameters p = new EccComponent.Parameters()
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Function = ParameterValidator.ValidFunctions,
                Curve = new string[] { "p-192" },
                IsSample = true
            };

            return CreateRegistration(folderName, p);
        }

        protected override string GetTestFileLotsOfTestCases(string folderName)
        {
            EccComponent.Parameters p = new EccComponent.Parameters()
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Function = ParameterValidator.ValidFunctions,
                Curve = new string[] { "p-192", "k-163", "b-163" },
                IsSample = true
            };

            return CreateRegistration(folderName, p);
        }
        
        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            // If TC has a z, change it
            if (testCase.z != null)
            {
                BitString bs = new BitString(testCase.z.ToString());
                bs = _random.GetDifferentBitStringOfSameSize(bs);

                // Can't get something "different" of empty bitstring of the same length
                if (bs == null)
                {
                    bs = new BitString("01");
                }

                testCase.z = bs.ToHex();
            }
        }
    }
}