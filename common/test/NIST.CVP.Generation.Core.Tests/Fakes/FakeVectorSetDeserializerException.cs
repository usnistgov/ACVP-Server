using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core.DeSerialization;

namespace NIST.CVP.Generation.Core.Tests.Fakes
{
    public class FakeVectorSetDeserializerException<TTestVectorSet, TTestGroup, TTestCase> : IVectorSetDeserializer<TTestVectorSet, TTestGroup, TTestCase>
        where TTestVectorSet : ITestVectorSet<TTestGroup, TTestCase>
        where TTestGroup : ITestGroup<TTestGroup, TTestCase>
        where TTestCase : ITestCase<TTestGroup, TTestCase>
    {
        public TTestVectorSet Deserialize(string vectorSetJson)
        {
            throw new Exception();
        }
    }
}
