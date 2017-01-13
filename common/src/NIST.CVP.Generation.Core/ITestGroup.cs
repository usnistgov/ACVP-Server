using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.Core
{
    public interface ITestGroup
    {
        string TestType { get; }
        List<ITestCase> Tests { get; }
        bool MergeTests(List<ITestCase> testsToMerge);
        int KeyLength { get; }
    }
}
