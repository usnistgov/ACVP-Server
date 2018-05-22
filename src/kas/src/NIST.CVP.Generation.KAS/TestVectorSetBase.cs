using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.Helpers;

namespace NIST.CVP.Generation.KAS
{
    public abstract class TestVectorSetBase<TTestGroup, TTestCase, TKasDsaAlgoAttributes> : ITestVectorSet<TTestGroup, TTestCase>
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase, TKasDsaAlgoAttributes>, new()
        where TTestCase : TestCaseBase<TTestGroup, TTestCase, TKasDsaAlgoAttributes>, new()
        where TKasDsaAlgoAttributes : IKasDsaAlgoAttributes
    {
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public bool IsSample { get; set; }

        public List<TTestGroup> TestGroups { get; set; } = new List<TTestGroup>();
    }
}