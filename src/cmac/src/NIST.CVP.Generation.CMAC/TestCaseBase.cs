using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.CMAC
{
    public abstract class TestCaseBase : ITestCase
    {
        public int TestCaseId { get; set; }
        public bool FailureTest { get; set; }
        public bool Deferred => false;
        public BitString Key { get; set; }
        public BitString Message { get; set; }
        public BitString Mac { get; set; }
        public string Result { get; set; }

        protected TestCaseBase()
        {
            
        }
        public abstract bool SetString(string name, string value);
        protected abstract void MapToProperties(dynamic data);
    }
}