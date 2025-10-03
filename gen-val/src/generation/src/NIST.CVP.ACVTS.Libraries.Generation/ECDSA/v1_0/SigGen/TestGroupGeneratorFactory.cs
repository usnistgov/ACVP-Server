using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;

namespace NIST.CVP.ACVTS.Libraries.Generation.ECDSA.v1_0.SigGen
{
    public class TestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters, TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public TestGroupGeneratorFactory(IOracle oracle)
        {
            _oracle = oracle;
        }

        public IEnumerable<ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>> GetTestGroupGenerators(Parameters parameters)
        {
            // TestGroupGeneratorFactory is used by 3X algos: ECDSA sigGen 1.0; ECDSA sigGen FIPS196-5, and DetECDSA sigGen FIPS196-5 
            var isDetECDSA = AlgoModeHelpers.GetAlgoModeFromAlgoAndMode(parameters.Algorithm, parameters.Mode, parameters.Revision) == AlgoMode.DetECDSA_SigGen_Fips186_5;
            // Special test cases that test for the correct padding of d in the seed material construction can only be created
            // if 1) deterministic ECDSA is being tested and 2) isSample = true. These special test cases can be created
            // when isSample = true, because ACVTS will create the key pair used for signing. When isSample = false,
            // ACVTS creates no key pair. The IUT signs the message provided by ACVTS and returns 1) the signature and
            // 2) the public key from the key pair used to sign the message.
            var testPaddingDInDetEcdsaPerMsgSecretNumberGeneration = parameters.IsSample && isDetECDSA;
            
            // Create a list of test group generators based on the options supplied in the registration
            HashSet<ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>> list = 
                new HashSet<ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>>();
            
            // 1) Standard test group generator. It will always be included.
            list.Add(new TestGroupGenerator(_oracle, false, false));
            
            // 2) If testPaddingDInDetECDSAPerMsgSecretNumberGeneration, i.e., if isSample && isDetECDSA, add a test group generator
            // that tests for the correct padding of d  
            if (testPaddingDInDetEcdsaPerMsgSecretNumberGeneration)
                list.Add(new TestGroupGenerator(_oracle, false, true));
            if (parameters.Conformances.Contains("SP800-106", StringComparer.OrdinalIgnoreCase))
            {
                // 3) If the IUT supports the SP800-106 conformance, add a test group generator to test randomized hashing for signatures
                list.Add(new TestGroupGenerator(_oracle, true, false));
                // 4) If testPaddingDInDetECDSAPerMsgSecretNumberGeneration, i.e., if isSample && isDetECDSA, add a test group generator
                // to test for the correct padding of d with randomized hashing for signatures
                if (testPaddingDInDetEcdsaPerMsgSecretNumberGeneration) 
                    list.Add(new TestGroupGenerator(_oracle, true, true));    
            }
            
            return list;
        }
    }
}
