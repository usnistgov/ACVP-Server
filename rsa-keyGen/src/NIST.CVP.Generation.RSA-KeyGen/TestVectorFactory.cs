using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA_KeyGen
{
    public class TestVectorFactory : ITestVectorFactory<Parameters>
    {
        private readonly IAlgorithmFunctionalTestGroupFactory<Parameters, TestGroup> _iAFTTestGroupFactory;
        private readonly IKnownAnswerTestGroupFactory<Parameters, TestGroup> _iKATTestGroupFactory;
        private readonly IGeneratedDataTestGroupFactory<Parameters, TestGroup> _iGDTTestGroupFactory;

        public TestVectorFactory(IAlgorithmFunctionalTestGroupFactory<Parameters, TestGroup> iAFTTestGroupFactory,
            IKnownAnswerTestGroupFactory<Parameters, TestGroup> iKATTestGroupFactory,
            IGeneratedDataTestGroupFactory<Parameters, TestGroup> iGDTTestGroupFactory)
        {
            _iAFTTestGroupFactory = iAFTTestGroupFactory;
            _iKATTestGroupFactory = iKATTestGroupFactory;
            _iGDTTestGroupFactory = iGDTTestGroupFactory;
        }

        public ITestVectorSet BuildTestVectorSet(Parameters parameters)
        {
            var groups = new List<ITestGroup>();

            var aftGroups = _iAFTTestGroupFactory.BuildAFTTestGroups(parameters);
            if (aftGroups != null && aftGroups.Count() != 0)
            {
                groups.AddRange(aftGroups);
            }

            var katGroups = _iKATTestGroupFactory.BuildKATTestGroups(parameters);
            if (katGroups != null && katGroups.Count() != 0)
            {
                groups.AddRange(katGroups);
            }

            var gdtGroups = _iGDTTestGroupFactory.BuildGDTTestGroups(parameters);
            if (gdtGroups != null && gdtGroups.Count() != 0)
            {
                groups.AddRange(gdtGroups);
            }

            return new TestVectorSet
            {
                TestGroups = groups,
                Algorithm = parameters.Algorithm,
                IsSample = parameters.IsSample
            };
        }
    }
}
