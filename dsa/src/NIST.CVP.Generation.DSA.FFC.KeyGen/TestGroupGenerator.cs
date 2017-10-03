using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.DSA.FFC.Enums;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;
using NLog;

namespace NIST.CVP.Generation.DSA.FFC.KeyGen
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

                FfcDomainParameters domainParams = null;
                if (parameters.IsSample)
                {
                    var domainParamsRequest = new FfcDomainParametersGenerateRequest(n, l, n, 256, null, PrimeGenMode.Provable, GeneratorGenMode.Unverifiable);
                    var domainParamsResult = _ffcDsa.GenerateDomainParameters(domainParamsRequest);

                    if (!domainParamsResult.Success)
                    {
                        ThisLogger.Error($"Failure generating domain parameters for L = {l}, N = {n}: {domainParamsResult.ErrorMessage}");
                        continue;
                    }

                    domainParams = domainParamsResult.PqgDomainParameters;
                }
                

                var testGroup = new TestGroup
                {
                    L = l,
                    N = n,
                    DomainParams = domainParams
                };

                testGroups.Add(testGroup);
            }

            return testGroups;
        }

        private Logger ThisLogger { get { return LogManager.GetCurrentClassLogger(); } }
    }
}
