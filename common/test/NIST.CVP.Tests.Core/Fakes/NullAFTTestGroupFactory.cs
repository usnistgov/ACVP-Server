using System;
using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Tests.Core.Fakes
{
    public class NullAFTTestGroupFactory<TParameters, TTestGroup> : IAlgorithmFunctionalTestGroupFactory<TParameters, TTestGroup>
        where TParameters : IParameters
        where TTestGroup : ITestGroup
    {
        public IEnumerable<TTestGroup> BuildAFTTestGroups(TParameters parameters)
        {
            return null;
        }
    }
}
