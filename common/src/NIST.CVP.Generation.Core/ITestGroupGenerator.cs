using System.Collections.Generic;

namespace NIST.CVP.Generation.Core
{
    public interface ITestGroupGenerator<in TParameters>
        where TParameters : IParameters
    {
        IEnumerable<ITestGroup> BuildTestGroups(TParameters parameters);
    }
}