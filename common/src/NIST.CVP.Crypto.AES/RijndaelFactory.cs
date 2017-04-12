using System;

namespace NIST.CVP.Crypto.AES
{
    public class RijndaelFactory : IRijndaelFactory
    {
        private readonly IRijndaelInternals _iRijndaelInternals;

        public RijndaelFactory(IRijndaelInternals iRijndaelInternals)
        {
            _iRijndaelInternals = iRijndaelInternals;
        }

        public Rijndael GetRijndael(ModeValues mode)
        {
            switch (mode)
            {
                case ModeValues.ECB:
                    return new RijndaelECB(_iRijndaelInternals);
                case ModeValues.CBC:
                    return new RijndaelCBC(_iRijndaelInternals);
                case ModeValues.OFB:
                    return new RijndaelOFB(_iRijndaelInternals);
                case ModeValues.CFB1:
                    return new RijndaelCFB1(_iRijndaelInternals);
                case ModeValues.CFB8:
                    return new RijndaelCFB8(_iRijndaelInternals);
                case ModeValues.CFB128:
                    return new RijndaelCFB128(_iRijndaelInternals);
                case ModeValues.Counter:
                    return new RijndaelCounter(_iRijndaelInternals);
                case ModeValues.CBCMac:
                    return new RijndaelCBCMac(_iRijndaelInternals);
                default:
                    throw new ArgumentException($"invalid value for {nameof(mode)}");
            }

        }
    }
}
