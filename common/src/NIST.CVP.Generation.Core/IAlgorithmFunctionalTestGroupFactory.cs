using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.Core
{
    [Obsolete("To be replaced by more generic ITestGroupGenerator")]
    public interface IAlgorithmFunctionalTestGroupFactory<in TParameters, out TTestGroup>
        where TParameters : IParameters
        where TTestGroup : ITestGroup
    {
        IEnumerable<TTestGroup> BuildAFTTestGroups(TParameters parameters);
    }
}
