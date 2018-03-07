using System.Dynamic;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_CCM
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public bool? TestPassed { get; set; }
        public bool Deferred { get; set; }
        public BitString PlainText { get; set; }
        public BitString Key { get; set; }
        public BitString AAD { get; set; }
        public BitString IV { get; set; }
        public BitString CipherText { get; set; }
        public TestGroup ParentGroup { get; set; }
        
        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }
      
            switch (name.ToLower())
            {
                case "key":
                    Key = new BitString(value);
                    return true;
                case "aad":
                case "adata":
                    AAD = new BitString(value);
                    return true;
                case "iv":
                case "nonce":
                    IV = new BitString(value);
                    return true;
                case "payload":
                case "plaintext":
                case "pt":
                    PlainText= new BitString(value);
                    return true;
                case "ciphertext":
                case "ct":
                    CipherText = new BitString(value);
                    return true;
            }
            return false;
        }
    }
}
