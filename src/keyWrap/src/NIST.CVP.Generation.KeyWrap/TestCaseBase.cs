using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KeyWrap
{
    public abstract class TestCaseBase : ITestCase
    {
        public int TestCaseId { get; set; }
        public bool FailureTest { get; set; }
        public bool Deferred { get; set; }
        public BitString Key { get; set; }
        public BitString PlainText { get; set; }
        public BitString CipherText { get; set; }

        public abstract bool SetString(string name, string value);
        protected abstract void MapToProperties(dynamic data);
    }
}
