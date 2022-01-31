using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.RSA.v1_0.SigVer.TestCaseExpectations;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.v1_0.SigVer
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public const string TEST_TYPE = "GDT";

        private readonly IOracle _oracle;
        private readonly bool _randomizeMessagePriorToSign;

        public TestGroupGenerator(IOracle oracle, bool randomizeMessagePriorToSign)
        {
            _oracle = oracle;
            _randomizeMessagePriorToSign = randomizeMessagePriorToSign;
        }

        public async Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
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
                                Standard = parameters.Fips186Standard
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
