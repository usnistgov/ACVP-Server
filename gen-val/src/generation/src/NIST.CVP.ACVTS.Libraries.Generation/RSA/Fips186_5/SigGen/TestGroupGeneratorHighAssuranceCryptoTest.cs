using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.Fips186_5.SigGen
{
    public class TestGroupGeneratorHighAssuranceCryptoTest : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public TestGroupGeneratorHighAssuranceCryptoTest(IOracle oracle)
        {
            _oracle = oracle;
        }

        public const string TEST_TYPE = "GDT";
        public const string INTERNAL_TEST_TYPE = "HACT";

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
                // If PKCS, skip
                if (capability.SigType == SignatureSchemes.Pkcs1v15) continue;

                foreach (var moduloCap in capability.ModuloCapabilities)
                {
                    // If not 2048, skip
                    if (moduloCap.Modulo != 2048) continue;

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
                            var testGroup = new TestGroup
                            {
                                Mode = capability.SigType,
                                Modulo = moduloCap.Modulo,
                                HashAlg = hashAlg,
                                SaltLen = hashPair.SaltLen,
                                MaskFunction = maskFunction,

                                TestType = TEST_TYPE,
                                InternalTestType = INTERNAL_TEST_TYPE
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
                // If PKCS, skip
                if (capability.SigType == SignatureSchemes.Pkcs1v15) continue;

                foreach (var moduloCap in capability.ModuloCapabilities)
                {
                    // If not 2048, skip
                    if (moduloCap.Modulo != 2048) continue;

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
                            var testGroup = new TestGroup
                            {
                                Mode = capability.SigType,
                                Modulo = moduloCap.Modulo,
                                HashAlg = hashAlg,
                                SaltLen = hashPair.SaltLen,
                                MaskFunction = maskFunction,

                                TestType = TEST_TYPE,
                                InternalTestType = INTERNAL_TEST_TYPE
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
