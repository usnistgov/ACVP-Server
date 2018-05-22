using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;

namespace NIST.CVP.Generation.KAS.FFC
{
    public class PqgProviderReuseGenerations : IPqgProvider
    {

        private readonly IDsaFfcFactory _dsaFactory;

        private static readonly Dictionary<(int p, int q, int shaOutputLength), FfcDomainParameters> _generatedPqg =
            new Dictionary<(int p, int q, int shaLength), FfcDomainParameters>();

        public PqgProviderReuseGenerations(IDsaFfcFactory dsaFactory)
        {
            _dsaFactory = dsaFactory;
        }

        public FfcDomainParameters GetPqg(int p, int q, HashFunction hashFunction)
        {
            var shaAttributes = ShaAttributes.GetShaAttributes(hashFunction.Mode, hashFunction.DigestSize);

            var generated =
                _generatedPqg
                    .FirstOrDefault(w =>
                        w.Key.p == p &&
                        w.Key.q == q &&
                        w.Key.shaOutputLength == shaAttributes.outputLen
                    )
                    .Value;

            if (generated != null)
            {
                return generated;
            }

            // Could not find pregenerated PQG for options, generate using DSA
            var dsa = _dsaFactory.GetInstance(hashFunction);
            var domainParams = dsa.GenerateDomainParameters(
                new FfcDomainParametersGenerateRequest(
                    q,
                    p,
                    q,
                    shaAttributes.outputLen,
                    null,
                    PrimeGenMode.Probable,
                    GeneratorGenMode.Unverifiable
                )
            ).PqgDomainParameters;

            _generatedPqg.Add((p, q, shaAttributes.outputLen), domainParams);

            return domainParams;
        }
    }
}