using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Generation.Core;
using NLog;

namespace NIST.CVP.Generation.RSA.Fips186_5.SigGen
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
            if (parameters.IsSample)
            {
                var groups = await BuildSampleTestGroupsAsync(parameters);

                return groups;
            } 
            
            return BuildGroups(parameters);
        }

        private async Task<List<TestGroup>> BuildSampleTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            // For samples we need to generate a key for the groups
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
                            var testGroup = new TestGroup
                            {
                                Mode = capability.SigType,
                                Modulo = moduloCap.Modulo,
                                HashAlg = ShaAttributes.GetHashFunctionFromName(hashPair.HashAlg),
                                SaltLen = hashPair.SaltLen,
                                Conformance = _randomizeMessagePriorToSign ? "SP800-106" : null,
                                MaskFunction = maskFunction,
                                
                                TestType = TEST_TYPE
                            };

                            var param = new RsaKeyParameters
                            {
                                KeyFormat = PrivateKeyModes.Standard,
                                Modulus = moduloCap.Modulo,
                                KeyMode = PrimeGenModes.RandomProbablePrimes,
                                PublicExponentMode = PublicExponentModes.Random,
                                PrimeTest = PrimeTestModes.TwoPow100ErrorBound,
                                Standard = Fips186Standard.Fips186_5
                            };

                            map.Add(testGroup, _oracle.GetRsaKeyAsync(param));
                        }
                    }
                }
            }

            await Task.WhenAll(map.Values);
            foreach (var (group, value) in map)
            {
                var key = value.Result;
                group.Key = key.Key;

                testGroups.Add(group);
            }

            return testGroups;
        }

        private List<TestGroup> BuildGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();
            
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
                            var testGroup = new TestGroup
                            {
                                Mode = capability.SigType,
                                Modulo = moduloCap.Modulo,
                                HashAlg = ShaAttributes.GetHashFunctionFromName(hashPair.HashAlg),
                                SaltLen = hashPair.SaltLen,
                                Conformance = _randomizeMessagePriorToSign ? "SP800-106" : null,
                                MaskFunction = maskFunction,
                                
                                TestType = TEST_TYPE
                            };

                            testGroups.Add(testGroup);
                        }
                    }
                }
            }

            return testGroups;
        }
    }
}