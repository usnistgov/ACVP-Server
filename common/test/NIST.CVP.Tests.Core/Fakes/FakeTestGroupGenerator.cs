using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Tests.Core.Fakes
{
    public class FakeTestGroupGenerator : ITestGroupGenerator<IParameters>
    {
        public IEnumerable<ITestGroup> BuildTestGroups(IParameters parameters)
        {
            return null;
        }
    }
}
