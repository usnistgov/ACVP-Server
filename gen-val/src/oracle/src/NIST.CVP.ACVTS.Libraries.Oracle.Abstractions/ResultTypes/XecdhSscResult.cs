using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes
{
    public class XecdhSscResult
    {

        public BitString PrivateKeyServer { get; set; }

        public BitString PublicKeyServer { get; set; }


        #region Sample Only Properties
        public BitString PrivateKeyIut { get; set; }

        public BitString PublicKeyIut { get; set; }

        public BitString Z { get; set; }
        #endregion Sample Only Properties
    }
}
