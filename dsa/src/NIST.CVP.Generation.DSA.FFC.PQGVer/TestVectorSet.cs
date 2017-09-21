using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.FFC.PQGVer
{
    public class TestVectorSet : ITestVectorSet
    {
        public string Algorithm { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Mode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsSample { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<ITestGroup> TestGroups { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public List<dynamic> AnswerProjection => throw new NotImplementedException();

        public List<dynamic> PromptProjection => throw new NotImplementedException();

        public List<dynamic> ResultProjection => throw new NotImplementedException();
    }
}
