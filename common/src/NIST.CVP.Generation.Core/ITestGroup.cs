using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.Core
{
    public interface ITestGroup
    {
        List<ITestCase> Tests { get; }
        bool MergeTests(List<ITestCase> testsToMerge);
    }
}
