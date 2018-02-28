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
    public abstract class LegacyResponseFileParserBase<TTestVectorSet, TTestGroup> : ILegacyResponseFileParser<TTestVectorSet>
        where TTestVectorSet : TestVectorSetBase<TTestGroup>, new()
        where TTestGroup : TestGroupBase, new()
    {
        public abstract ParseResponse<TTestVectorSet> Parse(string path);

        protected abstract TTestGroup CreateTestGroup(string path);
    }
}
