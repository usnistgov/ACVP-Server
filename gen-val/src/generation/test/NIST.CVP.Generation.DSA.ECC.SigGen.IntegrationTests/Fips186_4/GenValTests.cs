﻿using NIST.CVP.Common;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.ECDSA.v1_0.SigGen;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.ECC.SigGen.IntegrationTests.Fips186_4
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm { get; } = "ECDSA";
        public override string Mode { get; } = "SigGen";

        public override AlgoMode AlgoMode => AlgoMode.ECDSA_SigGen_v1_0;


        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            var caps = new[]
            {
                new Capability
                {
                    Curve = new[] { "p-224", "b-233" },
                    HashAlg = new[] { "sha2-224" }
                },
                new Capability
                {
                    Curve = new[] { "k-283" },
                    HashAlg = new[] {"sha2-512" }
                }
            };

            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                IsSample = true,
                Capabilities = caps,
                ComponentTest = true
            };

            return CreateRegistration(targetFolder, p);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            var caps = new[]
            {
                new Capability
                {
                    Curve = ParameterValidator.VALID_CURVES,
                    HashAlg = ParameterValidator.VALID_HASH_ALGS
                }
            };

            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                IsSample = true,
                Conformances = new[] { "SP800-106" },
                Capabilities = caps,
            };

            return CreateRegistration(targetFolder, p);
        }

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();
            if (testCase.r != null)
            {
                testCase.r = rand.GetDifferentBitStringOfSameSize(new BitString(testCase.r.ToString())).ToHex();
            }

            if (testCase.s != null)
            {
                testCase.s = rand.GetDifferentBitStringOfSameSize(new BitString(testCase.s.ToString())).ToHex();
            }
        }
    }
}