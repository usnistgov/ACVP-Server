using NIST.CVP.Crypto.Common.MAC.CMAC.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.CMAC
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public bool? TestPassed { get; set; }
        public bool Deferred => false;
        public TestGroup ParentGroup { get; set; }
        public BitString Key { get; set; }
        public BitString Message { get; set; }
        public BitString Mac { get; set; }

        #region Tdes only
        public BitString Key1
        {
            get => ParentGroup?.CmacType != CmacTypes.TDES ? null : Key?.MSBSubstring(0, 64);
            set => Key = ParentGroup?.CmacType != CmacTypes.TDES ? null : Key != null ? value.ConcatenateBits(Key.Substring(64, 128)) : null;
        }

        public BitString Key2
        {
            get => ParentGroup?.CmacType != CmacTypes.TDES ? null : Key?.MSBSubstring(64, 64);
            set => Key = ParentGroup?.CmacType != CmacTypes.TDES ? null : Key?.Substring(0, 64).ConcatenateBits(value).ConcatenateBits(Key.Substring(128, 64));
        }
        public BitString Key3
        {
            get => ParentGroup?.CmacType != CmacTypes.TDES ? null : Key?.BitLength > 128 ? Key?.MSBSubstring(128, 64) : Key?.MSBSubstring(0, 64);
            set => Key = ParentGroup?.CmacType != CmacTypes.TDES ? null : Key?.Substring(0, 128).ConcatenateBits(value);
        }
        #endregion Tdes only

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "msg":
                    Message = new BitString(value);
                    return true;
                case "mac":
                    Mac = new BitString(value);
                    return true;
                
                case "k":
                case "key":
                case "keys":
                    Key = new BitString(value);
                    return true;

                case "key1":
                    Key1 = new BitString(value);
                    return true;

                case "key2":
                    Key2 = new BitString(value);
                    return true;
            }
            return false;
        }
    }
}