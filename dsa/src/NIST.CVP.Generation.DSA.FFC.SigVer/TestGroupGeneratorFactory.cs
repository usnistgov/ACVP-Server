using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.FFC.SigVer
{
    public class TestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters>
    {
        private readonly IDsaFfcFactory _dsaFactory;

        public TestGroupGeneratorFactory(IDsaFfcFactory dsaFactory)
        {
            _dsaFactory = dsaFactory;
        }

        public IEnumerable<ITestGroupGenerator<Parameters>> GetTestGroupGenerators()
        {
            var list = new HashSet<ITestGroupGenerator<Parameters>>
            {
                new TestGroupGenerator(_dsaFactory)
            };

            return list;
        }
    }
}
