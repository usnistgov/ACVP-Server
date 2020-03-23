using NIST.CVP.Common.Helpers;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Generation.Core;
using NLog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.RSA.v1_0.SigGen
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        private readonly bool _randomizeMessagePriorToSign;

        public TestGroupGenerator(IOracle oracle, bool randomizeMessagePriorToSign)
        {
            _oracle = oracle;
            _randomizeMessagePriorToSign = randomizeMessagePriorToSign;
        }

        public const string TEST_TYPE = "GDT";

        public async Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            if (!parameters.IsSample)
            {
                foreach (var capability in parameters.Capabilities)
                {
                    var sigType = capability.SigType;

                    foreach (var moduloCap in capability.ModuloCapabilities)
                    {
                        var modulo = moduloCap.Modulo;

                        foreach (var hashPair in moduloCap.HashPairs)
                        {
                            var testGroup = new TestGroup
                            {
                                Mode = EnumHelpers.GetEnumFromEnumDescription<SignatureSchemes>(sigType),
                                Modulo = modulo,
                                HashAlg = ShaAttributes.GetHashFunctionFromName(hashPair.HashAlg),
                                SaltLen = hashPair.SaltLen,
                                Conformance = _randomizeMessagePriorToSign ? "SP800-106" : null,

                                TestType = TEST_TYPE
                            };

                            testGroups.Add(testGroup);
                        }
                    }
                }

                return testGroups;
            }

            // For samples we need to generate a key for the groups
            Dictionary<TestGroup, Task<RsaKeyResult>> map = new Dictionary<TestGroup, Task<RsaKeyResult>>();
            foreach (var capability in parameters.Capabilities)
            {
                var sigType = capability.SigType;

                foreach (var moduloCap in capability.ModuloCapabilities)
                {
                    var modulo = moduloCap.Modulo;

                    foreach (var hashPair in moduloCap.HashPairs)
                    {
                        var testGroup = new TestGroup
                        {
                            Mode = EnumHelpers.GetEnumFromEnumDescription<SignatureSchemes>(sigType),
                            Modulo = modulo,
                            HashAlg = ShaAttributes.GetHashFunctionFromName(hashPair.HashAlg),
                            SaltLen = hashPair.SaltLen,
                            Conformance = _randomizeMessagePriorToSign ? "SP800-106" : null,

                            TestType = TEST_TYPE
                        };

                        var param = new RsaKeyParameters
                        {
                            KeyFormat = PrivateKeyModes.Standard,
                            Modulus = modulo,
                            KeyMode = PrimeGenModes.RandomProbablePrimes,
                            PublicExponentMode = PublicExponentModes.Random,
                            PrimeTest = PrimeTestModes.TwoPow100ErrorBound,
                            Standard = Fips186Standard.Fips186_4
                        };

                        map.Add(testGroup, _oracle.GetRsaKeyAsync(param));
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
