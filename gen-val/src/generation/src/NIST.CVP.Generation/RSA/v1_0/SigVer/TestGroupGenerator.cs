﻿using NIST.CVP.Common.Helpers;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.RSA.v1_0.SigVer.TestCaseExpectations;
using NIST.CVP.Math;
using NLog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.RSA.v1_0.SigVer
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        public const string TEST_TYPE = "GDT";

        private readonly IOracle _oracle;
        private readonly bool _randomizeMessagePriorToSign;

        public TestGroupGenerator(IOracle oracle, bool randomizeMessagePriorToSign)
        {
            _oracle = oracle;
            _randomizeMessagePriorToSign = randomizeMessagePriorToSign;
        }

        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var groups = BuildTestGroupsAsync(parameters);
            groups.Wait();

            return groups.Result;
        }

        private async Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();
            var pubExpMode = EnumHelpers.GetEnumFromEnumDescription<PublicExponentModes>(parameters.PubExpMode);
            var keyFormat = EnumHelpers.GetEnumFromEnumDescription<PrivateKeyModes>(parameters.KeyFormat);
            Dictionary<TestGroup, Task<RsaKeyResult>> map = new Dictionary<TestGroup, Task<RsaKeyResult>>();

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
                                KeyMode = PrimeGenModes.RandomProbablePrimes,
                                PublicExponentMode = pubExpMode,
                                PublicExponent = !string.IsNullOrEmpty(parameters.FixedPubExpValue) ? new BitString(parameters.FixedPubExpValue) : null,
                                PrimeTest = PrimeTestModes.TwoPow100ErrorBound,
                                Standard = parameters.Legacy ? Fips186Standard.Fips186_2 : Fips186Standard.Fips186_4
                            };

                            var testGroup = new TestGroup
                            {
                                Mode = EnumHelpers.GetEnumFromEnumDescription<SignatureSchemes>(sigType),
                                Modulo = modulo,
                                HashAlg = ShaAttributes.GetHashFunctionFromName(hashPair.HashAlg),
                                SaltLen = hashPair.SaltLen,
                                TestCaseExpectationProvider = new TestCaseExpectationProvider(parameters.IsSample),
                                Conformance = _randomizeMessagePriorToSign ? "SP800-106" : null,

                                TestType = TEST_TYPE
                            };

                            map.Add(testGroup, _oracle.GetRsaKeyAsync(param));
                        }
                    }
                }
            }

            await Task.WhenAll(map.Values);
            foreach (var keyValuePair in map)
            {
                var group = keyValuePair.Key;
                var key = keyValuePair.Value.Result;
                group.Key = key.Key;

                testGroups.Add(group);
            }

            return testGroups;
        }

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}