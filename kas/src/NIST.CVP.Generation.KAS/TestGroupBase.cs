using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KAS
{
    public abstract class TestGroupBase : ITestGroup
    {
        public KasAssurance Function { get; set; }
        public string TestType { get; set; }
        public KeyAgreementRole KasRole { get; set; }
        public KasMode KasMode { get; set; }
        public HashFunction HashAlg { get; set; }
        public KeyAgreementMacType MacType { get; set; }
        public int KeyLen { get; set; }
        public int AesCcmNonceLen { get; set; }
        public int MacLen { get; set; }
        public string KdfType { get; set; }
        public int IdServerLen { get; set; } = 48;
        public BitString IdServer { get; set; } = new BitString("434156536964");
        public int IdIutLen { get; set; }
        public BitString IdIut { get; set; }
        public string OiPattern { get; set; }
        public KeyConfirmationRole KcRole { get; set; }
        public KeyConfirmationDirection KcType { get; set; }
        public string NonceType { get; set; }
        
        public List<ITestCase> Tests { get; set; } = new List<ITestCase>();
        public bool MergeTests(List<ITestCase> testsToMerge)
        {
            foreach (var test in Tests)
            {
                var matchingTest = testsToMerge.FirstOrDefault(t => t.TestCaseId == test.TestCaseId);
                if (matchingTest == null)
                {
                    return false;
                }
                if (!test.Merge(matchingTest))
                {
                    return false;
                }
            }
            return true;
        }

        public abstract override int GetHashCode();
        public abstract override bool Equals(object obj);
    }
}
