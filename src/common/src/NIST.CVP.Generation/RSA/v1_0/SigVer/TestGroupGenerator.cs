﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.RSA.v1_0.SigVer.TestCaseExpectations;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.RSA.v1_0.SigVer
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        public const string TEST_TYPE = "GDT";

        private readonly IOracle _oracle;

        public TestGroupGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var groups = BuildTestGroupsAsync(parameters);
            return groups.Result.ToArray();
        }

        private async Task<IEnumerable<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();
            var pubExpMode = EnumHelpers.GetEnumFromEnumDescription<PublicExponentModes>(parameters.PubExpMode);
            var keyFormat = EnumHelpers.GetEnumFromEnumDescription<PrivateKeyModes>(parameters.KeyFormat);

            foreach (var capability in parameters.Capabilities)
            {
                var sigType = capability.SigType;

                foreach (var moduloCap in capability.ModuloCapabilities)
                {
                    var modulo = moduloCap.Modulo;

                    foreach (var hashPair in moduloCap.HashPairs)
                    {
                        for (var i = 0; i < 3; i++)
                        {
                            var param = new RsaKeyParameters
                            {
                                KeyFormat = keyFormat,
                                Modulus = modulo,
                                KeyMode = PrimeGenModes.B33,
                                PublicExponentMode = pubExpMode,
                                PublicExponent = new BitString(parameters.FixedPubExpValue),
                                PrimeTest = PrimeTestModes.C2
                            };

                            try
                            {
                                var keyResult = await _oracle.GetRsaKeyAsync(param);

                                var testGroup = new TestGroup
                                {
                                    Mode = EnumHelpers.GetEnumFromEnumDescription<SignatureSchemes>(sigType),
                                    Modulo = modulo,
                                    HashAlg = ShaAttributes.GetHashFunctionFromName(hashPair.HashAlg),
                                    SaltLen = hashPair.SaltLen,
                                    Key = keyResult.Key,
                                    TestCaseExpectationProvider = new TestCaseExpectationProvider(parameters.IsSample),

                                    TestType = TEST_TYPE
                                };

                                testGroups.Add(testGroup);
                            }
                            catch (Exception ex)
                            {
                                ThisLogger.Warn($"Error generating key for {modulo}. {ex.Message}");
                            }
                        }
                    }
                }
            }

            return testGroups;
        }

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}