using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.CMAC.Parsers
{
    public abstract class LegacyResponseFileParserBase<TTestVectorSet, TTestGroup, TTestCase> : ILegacyResponseFileParser<TTestVectorSet, TTestGroup, TTestCase>
        where TTestVectorSet : TestVectorSetBase<TTestGroup, TTestCase>, new()
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase>, new()
        where TTestCase : TestCaseBase<TTestGroup, TTestCase>, new()
    {
        public abstract ParseResponse<TTestVectorSet> Parse(string path);

        protected abstract TTestGroup CreateTestGroup(string path);
    }
}
