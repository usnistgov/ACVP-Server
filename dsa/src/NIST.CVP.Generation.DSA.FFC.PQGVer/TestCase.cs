using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.FFC.PQGVer
{
    public class TestCase : ITestCase
    {
        public int TestCaseId => throw new NotImplementedException();

        public bool FailureTest => throw new NotImplementedException();

        public bool Deferred => throw new NotImplementedException();

        public bool Merge(ITestCase otherTest)
        {
            throw new NotImplementedException();
        }
    }
}
