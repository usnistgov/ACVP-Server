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
        private readonly IShaFactory _iShaFactory;
        

        public TestGroupGeneratorFactory(IDsaFfcFactory dsaFactory, IShaFactory iShaFactory)
        {
            _dsaFactory = dsaFactory;
            _iShaFactory = iShaFactory;
        }

        public IEnumerable<ITestGroupGenerator<Parameters>> GetTestGroupGenerators()
        {
            HashSet<ITestGroupGenerator<Parameters>> list =
                new HashSet<ITestGroupGenerator<Parameters>>()
                {
                    new TestGroupGenerator(_dsaFactory, _iShaFactory),
                };

            return list;
        }
    }
}
