using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.AES
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
                default:
                    throw new ArgumentException($"invalid value for {nameof(mode)}");
            }

        }
    }
}
