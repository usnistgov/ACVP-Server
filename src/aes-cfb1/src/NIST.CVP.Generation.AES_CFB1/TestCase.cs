using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Generation.AES_CFB1
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public bool? TestPassed => true;
        public bool Deferred => false;
        public TestGroup ParentGroup { get; set; }
        public int DataLen { get; set; }
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
                    ResultsArray[index].PlainText = new BitString(
                        MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed(value)
                    );
                    return true;

                case "ciphertext":
                case "ct":
                    ResultsArray[index].CipherText = new BitString(
                        MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed(value)
                    );
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
                    PlainText = new BitString(
                        MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed(value)
                    );
                    return true;

                case "ciphertext":
                case "ct":
                    CipherText = new BitString(
                        MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed(value)
                    );
                    return true;
            }
            return false;
        }
    }
}
