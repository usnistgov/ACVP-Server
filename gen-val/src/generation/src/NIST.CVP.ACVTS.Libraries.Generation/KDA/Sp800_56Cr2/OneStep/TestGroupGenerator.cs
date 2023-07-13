using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfOneStep;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDA.Sp800_56Cr2.OneStep
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        private readonly string[] _testTypes = { "AFT", "VAL" };

        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var groups = new List<TestGroup>();

            foreach (var testType in _testTypes)
            {
                foreach (var fixedInfoEncoding in parameters.Encoding)
                {
                    foreach (var auxFunction in parameters.AuxFunctions)
                    {
                        foreach (var saltMethod in GetSaltingMethods(auxFunction))
                        {
                            foreach (var zLength in GetZs(parameters.Z.GetDeepCopy()))
                            {
                                groups.Add(new TestGroup()
                                {
                                    KdfConfiguration = new OneStepConfiguration()
                                    {
                                        L = parameters.L,
                                        AuxFunction = auxFunction.AuxFunctionName,
                                        SaltMethod = saltMethod,
                                        SaltLen = GetSaltLen(auxFunction),
                                        FixedInfoEncoding = fixedInfoEncoding,
                                        FixedInfoPattern = parameters.FixedInfoPattern
                                    },
                                    TestType = testType,
                                    IsSample = parameters.IsSample,
                                    ZLength = zLength
                                });
                            }
                        }
                    }
                }
            }

            return Task.FromResult(groups);
        }

        private int GetSaltLen(AuxFunction auxFunction)
        {
            switch (auxFunction.AuxFunctionName)
            {
                case KdaOneStepAuxFunction.HMAC_SHA1:
                    return 512;
                case KdaOneStepAuxFunction.HMAC_SHA2_D224:
                    return 512;
                case KdaOneStepAuxFunction.HMAC_SHA2_D512_T224:
                    return 1024;
                case KdaOneStepAuxFunction.HMAC_SHA3_D224:
                    return 1152;
                case KdaOneStepAuxFunction.HMAC_SHA2_D256:
                    return 512;
                case KdaOneStepAuxFunction.HMAC_SHA2_D512_T256:
                    return 1024;
                case KdaOneStepAuxFunction.HMAC_SHA3_D256:
                    return 1088;
                case KdaOneStepAuxFunction.HMAC_SHA2_D384:
                    return 1024;
                case KdaOneStepAuxFunction.HMAC_SHA3_D384:
                    return 832;
                case KdaOneStepAuxFunction.HMAC_SHA2_D512:
                    return 1024;
                case KdaOneStepAuxFunction.HMAC_SHA3_D512:
                    return 576;
                case KdaOneStepAuxFunction.KMAC_128:
                    return 1312;
                case KdaOneStepAuxFunction.KMAC_256:
                    return 1056;
            }

            return 0;
        }

        private List<MacSaltMethod> GetSaltingMethods(AuxFunction auxFunction)
        {
            var results = new List<MacSaltMethod>();

            if (auxFunction.MacSaltMethods == null || auxFunction.MacSaltMethods.Length == 0)
            {
                results.Add(MacSaltMethod.None);
            }

            results.AddRangeIfNotNullOrEmpty(auxFunction.MacSaltMethods);

            return results;
        }

        private List<int> GetZs(MathDomain z)
        {
            var values = new List<int>();

            // Only one shared secret length is supported. Only need one test group
            if (z.GetDomainMinMax().Minimum == z.GetDomainMinMax().Maximum)
            {
                values.Add(z.GetDomainMinMax().Minimum);
            }
            else
            {
                values.AddRange(z.GetValues(i => i < 1024, 10, false));
                values.AddRange(z.GetValues(i => i < 4098, 5, false));
                values.AddRange(z.GetValues(i => i < 8196, 2, false));
                values.AddRange(z.GetValues(1));

                values = values.Shuffle().Take(3).ToList();
            
                values.Add(z.GetDomainMinMax().Minimum);
                values.Add(z.GetDomainMinMax().Maximum);                
            }
            
            return values.Shuffle().Take(5).ToList();
        }
    }
}
