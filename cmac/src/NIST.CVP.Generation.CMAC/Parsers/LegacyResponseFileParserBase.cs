using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NIST.CVP.Crypto.CMAC.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.CMAC.Parsers
{
    public abstract class LegacyResponseFileParserBase<TTestVectorSet, TTestGroup, TTestCase> : ILegacyResponseFileParser<TTestVectorSet>
        where TTestVectorSet : TestVectorSetBase<TTestGroup, TTestCase>, new()
        where TTestGroup : TestGroupBase<TTestCase>, new()
        where TTestCase : TestCaseBase, new()
    {
        public abstract ParseResponse<TTestVectorSet> Parse(string path);

        protected abstract TTestGroup CreateTestGroup(string path);
    }
}
