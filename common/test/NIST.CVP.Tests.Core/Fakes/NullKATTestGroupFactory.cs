using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Tests.Core.Fakes
{
    public class NullKATTestGroupFactory : IKATTestGroupFactory<IParameters, IEnumerable<ITestGroup>>
    {
        public IEnumerable<ITestGroup> BuildKATTestGroups(IParameters parameters)
        {
            return null;
        }
    }
}
