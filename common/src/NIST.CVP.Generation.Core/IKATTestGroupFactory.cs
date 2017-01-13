using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.Core
{
    public interface IKATTestGroupFactory<in TParameters, out TTestGroup>
        where TParameters : IParameters
        where TTestGroup : IEnumerable<ITestGroup>
    {
        TTestGroup BuildKATTestGroups(TParameters parameters);
    }
}
