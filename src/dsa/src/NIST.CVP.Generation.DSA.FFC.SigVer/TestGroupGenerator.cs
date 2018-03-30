using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.DSA.FFC.Helpers;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.DSA.FFC.SigVer.FailureHandlers;
using NLog;

namespace NIST.CVP.Generation.DSA.FFC.SigVer
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters>
    {
        private IShaFactory _shaFactory = new ShaFactory();
        private IDsaFfc _ffcDsa;

        public TestGroupGenerator(IDsaFfc ffcDsa = null)
        {
            if (ffcDsa == null)
            {
                _ffcDsa = new FfcDsa(_shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d256)));
            }
            else
            {
                _ffcDsa = ffcDsa;
            }
        }

        public IEnumerable<ITestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var capability in parameters.Capabilities)
            {
                var n = capability.N;
                var l = capability.L;

                foreach (var hashAlg in capability.HashAlg)
                {
                    FfcDomainParameters domainParams = null;
                    var domainParamsRequest = new FfcDomainParametersGenerateRequest(n, l, n, 256, null, PrimeGenMode.Provable, GeneratorGenMode.Unverifiable);
                    var domainParamsResult = _ffcDsa.GenerateDomainParameters(domainParamsRequest);

                    if (!domainParamsResult.Success)
                    {
                        ThisLogger.Error($"Failure generating domain parameters for L = {l}, N = {n}: {domainParamsResult.ErrorMessage}");
                        continue;
                    }

                    domainParams = domainParamsResult.PqgDomainParameters;

                    var shaAttributes = AlgorithmSpecificationToDomainMapping.GetMappingFromAlgorithm(hashAlg);
                    var sha = new HashFunction(shaAttributes.shaMode, shaAttributes.shaDigestSize);

                    var testGroup = new TestGroup
                    {
                        L = l,
                        N = n,
                        HashAlg = sha,
                        DomainParams = domainParams,
                        TestCaseExpectationProvider = new TestCaseExpectationProvider(parameters.IsSample)
                    };

                    testGroups.Add(testGroup);
                }
            }

            return testGroups;
        }

        private Logger ThisLogger { get { return LogManager.GetCurrentClassLogger(); } }
    }
}
