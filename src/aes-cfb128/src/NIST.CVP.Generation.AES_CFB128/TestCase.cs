using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_CFB128
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public bool? TestPassed => true;
        public bool Deferred { get; set; }
        public TestGroup ParentGroup { get; set; }
        public BitString IV { get; set; } 
        public BitString PlainText { get; set; }
        public BitString Key { get; set; }
        public BitString CipherText { get; set; }
        public List<AlgoArrayResponse> ResultsArray { get; set; }
        
        public bool SetResultsArrayString(int index, string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "key":
                    ResultsArray[index].Key = new BitString(value);
                    return true;

                case "iv":
                    ResultsArray[index].IV = new BitString(value);
                    return true;

                case "plaintext":
                case "pt":
                    ResultsArray[index].PlainText = new BitString(value);
                    return true;

                case "ciphertext":
                case "ct":
                    ResultsArray[index].CipherText = new BitString(value);
                    return true;
            }
            return false;
        }

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

                case "iv":
                    IV = new BitString(value);
                    return true;

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
