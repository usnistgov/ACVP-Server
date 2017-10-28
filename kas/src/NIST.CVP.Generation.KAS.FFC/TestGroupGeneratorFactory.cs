using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KAS.FFC
{
    public class TestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters>
    {
        private readonly IDsaFfcFactory _dsaFactory;
        private readonly IPqgProvider _iPqgProvider;
        

        public TestGroupGeneratorFactory(IDsaFfcFactory dsaFactory, IPqgProvider iPqgProvider)
        {
            _dsaFactory = dsaFactory;
            _iPqgProvider = iPqgProvider;
        }

        public IEnumerable<ITestGroupGenerator<Parameters>> GetTestGroupGenerators()
        {
            HashSet<ITestGroupGenerator<Parameters>> list =
                new HashSet<ITestGroupGenerator<Parameters>>()
                {
                    new TestGroupGenerator(_dsaFactory, _iPqgProvider),
                };

            return list;
        }
    }
}
