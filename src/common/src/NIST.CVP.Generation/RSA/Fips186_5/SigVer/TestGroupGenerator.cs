using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.RSA.Fips186_5.SigVer.TestCaseExpectations;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.RSA.Fips186_5.SigVer
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
            var keyFormat = EnumHelpers.GetEnumFromEnumDescription<PrivateKeyModes>(parameters.KeyFormat);
            var map = new Dictionary<TestGroup, Task<RsaKeyResult>>();

            foreach (var capability in parameters.Capabilities)
            {
                foreach (var moduloCap in capability.ModuloCapabilities)
                {
                    foreach (var hashPair in moduloCap.HashPairs)
                    {
                        if (capability.SigType != SignatureSchemes.Pss)
                        {
                            moduloCap.MaskFunction = new [] {PssMaskTypes.None};
                        }
                        
                        foreach (var maskFunction in moduloCap.MaskFunction)
                        {
                            for (var i = 0; i < 3; i++)
                            {
                                var param = new RsaKeyParameters
                                {
                                    KeyFormat = keyFormat,
                                    Modulus = moduloCap.Modulo,
                                    KeyMode = PrimeGenModes.RandomProbablePrimes,
                                    PublicExponentMode = parameters.PubExpMode,
                                    PublicExponent = parameters.FixedPubExpValue,
                                    PrimeTest = PrimeTestModes.TwoPow100ErrorBound,
                                    Standard = Fips186Standard.Fips186_5
                                };

                                var testGroup = new TestGroup
                                {
                                    Mode = capability.SigType,
                                    Modulo = moduloCap.Modulo,
                                    HashAlg = ShaAttributes.GetHashFunctionFromName(hashPair.HashAlg),
                                    SaltLen = hashPair.SaltLen,
                                    TestCaseExpectationProvider = new TestCaseExpectationProvider(parameters.IsSample),
                                    MaskFunction = maskFunction,
                                    Conformance = _randomizeMessagePriorToSign ? "SP800-106" : null,

                                    TestType = TEST_TYPE
                                };

                                map.Add(testGroup, _oracle.GetRsaKeyAsync(param));
                            } 
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