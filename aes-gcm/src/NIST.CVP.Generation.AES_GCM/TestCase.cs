using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_GCM
{
    public class TestCase : ITestCase
    {

        public TestCase()
        {
            
        }

        public TestCase(dynamic source)
        {
            TestCaseId = source.tcId;
           
            if (((ExpandoObject)source).ContainsProperty("failureTest"))
            {
                FailureTest = source.failureTest;
            }
            if (((ExpandoObject)source).ContainsProperty("deferred"))
            {
                Deferred = source.deferred;
            }
            if (((ExpandoObject)source).ContainsProperty("key"))
            {
                Key = source.key;
            }
            if (((ExpandoObject)source).ContainsProperty("iv"))
            {
                IV = source.iv;
            }
            if (((ExpandoObject) source).ContainsProperty("tag"))
            {
                Tag = source.tag;
            }
            if(((ExpandoObject)source).ContainsProperty("aad"))
            { 
                AAD = source.aad;
            }
            if (((ExpandoObject)source).ContainsProperty("cipherText"))
            {
                CipherText = source.cipherText;
            }
            if (((ExpandoObject)source).ContainsProperty("plainText"))
            {
                PlainText = source.plainText;
            }
        }
        public int TestCaseId { get; set; }
        public bool FailureTest { get; set; }
        public bool Deferred { get; set; }
        public BitString PlainText { get; set; }
        public BitString Key { get; set; }
        public BitString AAD { get; set; }
        public BitString IV { get; set; }
        public BitString CipherText { get; set; }
        public BitString Tag { get; set; }

        public bool Merge(ITestCase otherTest)
        {
            if (TestCaseId != otherTest.TestCaseId)
            {
                return false;
            }
            var otherTypedTest = (TestCase) otherTest;

            if (PlainText == null && otherTypedTest.PlainText != null)
            {
                AAD = otherTypedTest.AAD;
                PlainText = otherTypedTest.PlainText;
                return true;
            }

            if (CipherText == null && otherTypedTest.CipherText != null)
            {
                Tag = otherTypedTest.Tag;
                CipherText = otherTypedTest.CipherText;
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
                case "aad":
                    AAD = new BitString(value);
                    return true;
                case "tag":
                    Tag= new BitString(value);
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
