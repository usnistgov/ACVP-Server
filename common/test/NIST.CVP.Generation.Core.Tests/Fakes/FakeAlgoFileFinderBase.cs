using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.Core.Tests.Fakes
{
    public class FakeAlgoFileFinderBase : AlgoFileFinderBase
    {
        public override string Name { get { return "FakeAlgo"; } }
        public override string FilePrefix { get; }

        public FakeAlgoFileFinderBase(string filePrefix)
        {
            FilePrefix = filePrefix;
        }
    }
}
