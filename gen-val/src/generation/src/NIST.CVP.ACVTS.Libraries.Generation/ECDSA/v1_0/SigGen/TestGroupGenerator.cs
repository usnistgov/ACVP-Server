using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.ECDSA.v1_0.SigGen
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        private readonly bool _randomizeMessagePriorToSign;
        private readonly bool _testPaddingDInDetECDSAPerMsgSecretNumberGeneration;

        public TestGroupGenerator(IOracle oracle, bool randomizeMessagePriorToSign, bool testPaddingDInDetECDSAPerMsgSecretNumberGeneration)
        {
            _oracle = oracle;
            _randomizeMessagePriorToSign = randomizeMessagePriorToSign;
            _testPaddingDInDetECDSAPerMsgSecretNumberGeneration = testPaddingDInDetECDSAPerMsgSecretNumberGeneration;
        }

        public async Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            // Use a hash set because the registration allows for duplicate pairings to occur
            // Equality of groups is done via name of the curve and name of the hash function.
            // HashSet eliminates any duplicates that may be registered
            var testGroups = new HashSet<TestGroup>();

            if (!parameters.IsSample)
            {
                foreach (var capability in parameters.Capabilities)
                {
                    foreach (var curveName in capability.Curve)
                    {
                        foreach (var hashAlg in capability.HashAlg)
                        {
                            HashFunction sha = hashAlg.Contains("SHAKE") ? ShaAttributes.GetXofPssHashFunctionFromName(hashAlg) :
                                                                           ShaAttributes.GetHashFunctionFromName(hashAlg);
                            var curve = EnumHelpers.GetEnumFromEnumDescription<Curve>(curveName);

                            var testGroup = new TestGroup
                            {
                                Curve = curve,
                                HashAlg = sha,
                                ComponentTest = parameters.ComponentTest,
                                Conformance = _randomizeMessagePriorToSign ? "SP800-106" : null
                            };

                            testGroups.Add(testGroup);
                        }
                    }
                }

                return testGroups.ToList();
            }
            
            // For samples, we need to generate keys up front
            Dictionary<TestGroup, Task<EcdsaKeyResult>> map = new Dictionary<TestGroup, Task<EcdsaKeyResult>>();

            foreach (var capability in parameters.Capabilities)
            {
                foreach (var curveName in capability.Curve)
                {
                    foreach (var hashAlg in capability.HashAlg)
                    { 
                       HashFunction sha = hashAlg.Contains("SHAKE") ? ShaAttributes.GetXofPssHashFunctionFromName(hashAlg) :
                                                                      ShaAttributes.GetHashFunctionFromName(hashAlg);
                       var curve = EnumHelpers.GetEnumFromEnumDescription<Curve>(curveName);

                        var param = new EcdsaKeyParameters
                        {
                            Curve = curve,
                            // Do we need to intentionally generate a key pair that has a small d? I.e., are we trying to 
                            // test a deterministic ECDSA IUT's ability to properly left-pad the octet string representation
                            // of d? This is something that is done as part of calculating the per-message secret number.
                            // The octet string representation of d is left-padded with zeros until it is 8 * ceil( nlen/8 ) bits in length 
                            GenerateKeyPairWithSmallRandomD = _testPaddingDInDetECDSAPerMsgSecretNumberGeneration
                        };
                        
                        var testGroup = new TestGroup
                        {
                            Curve = curve,
                            HashAlg = sha,
                            ComponentTest = parameters.ComponentTest,
                            Conformance = _randomizeMessagePriorToSign ? "SP800-106" : null,
                            // _testPaddingDInDetECDSAPerMsgSecretNumberGeneration is only relevant, i.e., it will only be
                            // true for DetECDSA sigGen (vs ECDSA sigGen *); and it will only be true if isSample. ACVTS
                            // generates and supplies the private key when isSample vs when !isSample the IUT uses whatever
                            // private key it possesses to sign the message and provides ACVTS with the associated public
                            // key in the response so the signature for the message can be verified as being correct.
                            TestPaddingDInDetECDSAPerMsgSecretNumberGeneration = _testPaddingDInDetECDSAPerMsgSecretNumberGeneration
                        };

                        map.TryAdd(testGroup, _oracle.GetEcdsaKeyAsync(param));
                    }
                }
            }

            await Task.WhenAll(map.Values);

            foreach (var keyValuePair in map)
            {
                var group = keyValuePair.Key;
                var keyPair = keyValuePair.Value.Result;
                group.KeyPair = keyPair.Key;

                testGroups.Add(group);
            }

            return testGroups.ToList();
        }
    }
}
