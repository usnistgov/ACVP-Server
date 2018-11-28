using NIST.CVP.Common.Helpers;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Generation.Core;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.RSA_SigGen
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public TestGroupGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public const string TEST_TYPE = "GDT";

        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var groups = BuildTestGroupsAsync(parameters);
            groups.Wait();
            return groups.Result;
        }

        public async Task<IEnumerable<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

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

                            TestType = TEST_TYPE
                        };

                        // Make a single key for the group
                        if (parameters.IsSample)
                        {
                            var param = new RsaKeyParameters
                            {
                                KeyFormat = PrivateKeyModes.Standard,
                                Modulus = modulo,
                                KeyMode = PrimeGenModes.B33,
                                PublicExponentMode = PublicExponentModes.Random,
                                PrimeTest = PrimeTestModes.C2
                            };

                            try
                            {
                                var keyResult = await _oracle.GetRsaKeyAsync(param);
                                testGroup.Key = keyResult.Key;
                            }
                            catch (Exception ex)
                            {
                                ThisLogger.Warn($"Error generating key for {modulo}. {ex.Message}");
                            }
                        }

                        testGroups.Add(testGroup);
                    }
                }
            }

            return testGroups;
        }

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
