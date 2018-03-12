using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.Helpers;

namespace NIST.CVP.Generation.CMAC
{
    public abstract class TestVectorSetBase<TTestGroup, TTestCase> : ITestVectorSet<TTestGroup, TTestCase>
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase>
        where TTestCase : TestCaseBase<TTestGroup, TTestCase>
    {
        public abstract string Algorithm { get; set; }
        public abstract string Mode { get; set; }
        public bool IsSample { get; set; }
        public List<TTestGroup> TestGroups { get; set; } = new List<TTestGroup>();
    }
}