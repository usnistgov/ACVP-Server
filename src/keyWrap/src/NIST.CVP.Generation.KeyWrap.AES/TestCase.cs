using NIST.CVP.Math;

namespace NIST.CVP.Generation.KeyWrap.AES
{
    public class TestCase : TestCaseBase<TestGroup, TestCase>
    {
        public override bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }
      
            switch (name.ToLower())
            {
                case "pt":
                case "plaintext":
                case "p":
                    PlainText = new BitString(value);
                    return true;
                case "ct":
                case "ciphertext":
                case "c":
                    CipherText = new BitString(value);
                    return true;
                case "key":
                case "k":
                    Key = new BitString(value);
                    return true;
            }
            return false;
        }
    }
}
