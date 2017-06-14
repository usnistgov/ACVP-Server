using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Tests.Core.Fakes
{
    public class FakeParameters : IParameters
    {
        public string Algorithm { get; set; } = "test";
        public bool IsSample { get; set; } = true;
    }
}
