using System.Collections.Generic;
using NIST.CVP.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.FixedInfo
{
    public class FixedInfo : IFixedInfo
    {
        private readonly Dictionary<string, BitString> _fixedInfoParts = new Dictionary<string, BitString>();
        
        public BitString Get(FixedInfoParameter param)
        {
            throw new System.NotImplementedException();
        }
    }
}