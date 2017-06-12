using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA2
{
    public class TestVectorFactory : ITestVectorFactory<Parameters>
    {
        private readonly IAlgorithmFunctionalTestGroupFactory<Parameters, TestGroup> _iAFTTestGroupFactory;
        private readonly IMonteCarloTestGroupFactory<Parameters, TestGroup> _iMCTTestGroupFactory;

        public TestVectorFactory(IAlgorithmFunctionalTestGroupFactory<Parameters, TestGroup> iAFTTestGroupFactory,
            IMonteCarloTestGroupFactory<Parameters, TestGroup> iMCTTestGroupFactory)
        {
            _iAFTTestGroupFactory = iAFTTestGroupFactory;
            _iMCTTestGroupFactory = iMCTTestGroupFactory;
        }

        public ITestVectorSet BuildTestVectorSet(Parameters parameters)
        {
            var groups = new List<ITestGroup>();

            var aftGroups = _iAFTTestGroupFactory.BuildAFTTestGroups(parameters);
            if (aftGroups != null && aftGroups.Count() != 0)
            {
                groups.AddRange(aftGroups);
            }

            var mctGroups = _iMCTTestGroupFactory.BuildMCTTestGroups(parameters);
            if (mctGroups != null && mctGroups.Count() != 0)
            {
                groups.AddRange(mctGroups);
            }

            return new TestVectorSet {TestGroups = groups, Algorithm = parameters.Algorithm, IsSample = parameters.IsSample};
        }


    }
}
