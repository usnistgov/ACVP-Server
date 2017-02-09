using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.Core
{
    public interface IKnownAnswerTestGroupFactory<in TParameters, out TTestGroup>
        where TParameters : IParameters
        where TTestGroup : ITestGroup
    {
        IEnumerable<TTestGroup> BuildKATTestGroups(TParameters parameters);
    }
}
