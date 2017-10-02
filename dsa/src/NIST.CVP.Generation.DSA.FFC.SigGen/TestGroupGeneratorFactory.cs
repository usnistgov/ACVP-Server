using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.FFC.SigGen
{
    public class TestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters>
    {
        private IDsaFfc _ffcDsa;

        public TestGroupGeneratorFactory(IDsaFfc ffcDsa = null)
        {
            _ffcDsa = ffcDsa;
        }

        public IEnumerable<ITestGroupGenerator<Parameters>> GetTestGroupGenerators()
        {
            var list = new HashSet<ITestGroupGenerator<Parameters>>
            {
                new TestGroupGenerator(_ffcDsa)
            };

            return list;
        }
    }
}
