﻿using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Common;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Generation.RSA_SigVer;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.RSA_LegacySigVer.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm { get; } = "RSA";
        public override string Mode { get; } = "LegacySigVer";

        public override AlgoMode AlgoMode => AlgoMode.RSA_LegacySigVer;

        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();
		public override IRegisterInjections RegistrationsCrypto => new Crypto.RegisterInjections();

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            // If TC has a result, change it
            if (testCase.testPassed != null)
            {
                testCase.testPassed = !(bool) testCase.testPassed;
            }
        }

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            var hashPairs = new HashPair[2];
            for (var i = 0; i < hashPairs.Length; i++)
            {
                hashPairs[i] = new HashPair
                {
                    HashAlg = ParameterValidator.VALID_HASH_ALGS[i + 2],
                    SaltLen = i + 1
                };
            }

            var modCap = new CapSigType[1];
            modCap[0] = new CapSigType
            {
                Modulo = 2048,
                HashPairs = hashPairs
            };

            var algSpecs = new AlgSpecs[ParameterValidator.VALID_SIG_VER_MODES.Length];
            for (var i = 0; i < algSpecs.Length; i++)
            {
                algSpecs[i] = new AlgSpecs
                {
                    SigType = ParameterValidator.VALID_SIG_VER_MODES[i],
                    ModuloCapabilities = modCap
                };
            }

            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                IsSample = false,
                Capabilities = algSpecs,
                PubExpMode = "fixed",
                FixedPubExpValue = "11",
                KeyFormat = "standard"
            };

            return CreateRegistration(targetFolder, p);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            var hashPairs = new HashPair[ParameterValidator.VALID_HASH_ALGS.Length];
            for (var i = 0; i < hashPairs.Length; i++)
            {
                hashPairs[i] = new HashPair
                {
                    HashAlg = ParameterValidator.VALID_HASH_ALGS[i],
                    SaltLen = i + 8
                };
            }

            var modCap = new CapSigType[ParameterValidator.VALID_MODULI.Length];
            for (var i = 0; i < modCap.Length; i++)
            {
                modCap[i] = new CapSigType
                {
                    Modulo = ParameterValidator.VALID_MODULI[i],
                    HashPairs = hashPairs
                };
            }

            var algSpecs = new AlgSpecs[ParameterValidator.VALID_SIG_VER_MODES.Length];
            for (var i = 0; i < algSpecs.Length; i++)
            {
                algSpecs[i] = new AlgSpecs
                {
                    SigType = ParameterValidator.VALID_SIG_VER_MODES[i],
                    ModuloCapabilities = modCap
                };
            }

            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                IsSample = false,
                Capabilities = algSpecs,
                PubExpMode = "random",
                KeyFormat = "crt"
            };

            return CreateRegistration(targetFolder, p);
        }
    }
}