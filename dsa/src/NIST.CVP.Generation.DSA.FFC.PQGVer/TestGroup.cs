using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.FFC.PQGVer
{
    public class TestGroup : ITestGroup
    {
        public string TestType => throw new NotImplementedException();

        public List<ITestCase> Tests => throw new NotImplementedException();

        public bool MergeTests(List<ITestCase> testsToMerge)
        {
            throw new NotImplementedException();
        }
    }
}
