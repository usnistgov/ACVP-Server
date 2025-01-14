using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.Ed.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.EDDSA.v1_0.SigGen
{
    public class TestGroupGeneratorBitFlip : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        private const string TEST_TYPE = "BFT";

        private readonly IOracle _oracle;
        private static Random800_90 _rand = new Random800_90();
        private const int BITS_IN_BYTE = 8;
        private enum EddsaSignatureTypes
        {
            Pure,
            PreHash
        }

        public TestGroupGeneratorBitFlip(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var supportedSignatureTypes = new List<EddsaSignatureTypes>();
            if (parameters.Pure) supportedSignatureTypes.Add(EddsaSignatureTypes.Pure);
            if (parameters.PreHash) supportedSignatureTypes.Add(EddsaSignatureTypes.PreHash);
            
            // Use a hash set because the registration allows for duplicate pairings to occur
            // Equality of groups is done via name of the curve and name of the hash function.
            // HashSet eliminates any duplicates that may be registered
            var testGroups = new HashSet<TestGroup>();

            foreach (var signatureType in supportedSignatureTypes)
            {
                foreach (var curveName in parameters.Curve)
                {
                    var curve = EnumHelpers.GetEnumFromEnumDescription<Curve>(curveName);

                    EdKeyPair key = null;
                    var param = new EddsaKeyParameters
                    {
                        Curve = curve
                    };

                    if (parameters.IsSample)
                    {
                        var keyResult = await _oracle.GetEddsaKeyAsync(param);
                        key = keyResult.Key;
                    }

                    var paramMsg = new EddsaMessageParameters
                    {
                        IsSample = parameters.IsSample
                    };

                    var message = await _oracle.GetEddsaMessageBitFlipAsync(paramMsg);
                    
                    // context is applicable except for when the ED-25519 curve is used with the pure signature type 
                    var noContext = curve == Curve.Ed25519 && signatureType == EddsaSignatureTypes.Pure;
                    var contextLength = 0;
                    if (!noContext)
                    {
                        contextLength = parameters.ContextLength.GetRandomValues(
                        parameters.ContextLength.GetDomainMinMax().Minimum,
                        parameters.ContextLength.GetDomainMinMax().Maximum, 1).First();
                    }
                    var context =  noContext ? new BitString("") : _rand.GetRandomBitString(contextLength * BITS_IN_BYTE);

                    if (signatureType == EddsaSignatureTypes.Pure)
                    {
                        var testGroup = new TestGroup
                        {
                            Curve = curve,
                            PreHash = false,
                            KeyPair = key,
                            Message = message,
                            TestType = TEST_TYPE,
                            Context = context,
                        };
                        testGroups.Add(testGroup);
                    }

                    if (signatureType == EddsaSignatureTypes.PreHash)
                    {
                        var testGroup = new TestGroup
                        {
                            Curve = curve,
                            PreHash = true,
                            KeyPair = key,
                            Message = message,
                            TestType = TEST_TYPE,
                            Context = context,
                        };
                        testGroups.Add(testGroup);
                    }
                }                
            }

            return testGroups.ToList();
        }
    }
}
