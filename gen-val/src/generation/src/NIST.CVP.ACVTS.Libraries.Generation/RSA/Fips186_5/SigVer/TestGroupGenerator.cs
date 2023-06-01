using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.RSA.Fips186_5.SigVer.TestCaseExpectations;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.Fips186_5.SigVer
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
                            moduloCap.MaskFunction = new[] { PssMaskTypes.None };
                        }

                        HashFunction hashAlg;
                        // SHAKE 128 and 256 are only valid "Hashes" for PSS. When the SHAKEs are used in PSS, we 
                        // need to use the outputLens of 256 and 512 that are specified in FIPS 186-5 vs the outputLens
                        // of 128 and 256 that are used by default throughout the rest of the codebase.
                        if (ParameterValidator.VALID_XOF_ALGS.Contains(hashPair.HashAlg))
                        {
                            hashAlg = ShaAttributes.GetXofPssHashFunctionFromName(hashPair.HashAlg);
                        }
                        else
                        {
                            hashAlg = ShaAttributes.GetHashFunctionFromName(hashPair.HashAlg);
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
                                    HashAlg = hashAlg,
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
