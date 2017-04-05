using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Tests.Core.Fakes
{
    public class NullKATTestGroupFactory<TParameters, TTestGroup> : IKnownAnswerTestGroupFactory<TParameters, TTestGroup>
        where TParameters : IParameters
        where TTestGroup : ITestGroup
    {
        public IEnumerable<TTestGroup> BuildKATTestGroups(TParameters parameters)
        {
            return null;
        }
    }
}
