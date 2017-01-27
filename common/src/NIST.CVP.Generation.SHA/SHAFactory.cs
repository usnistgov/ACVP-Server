using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.SHA
{
    public class SHAFactory : ISHAFactory
    {
        public SHA GetSHA(ModeValues mode)
        {
            switch (mode)
            {
                case ModeValues.SHA1:
                    return new SHA1();
                case ModeValues.SHA2:
                case ModeValues.SHA2t:
                     return new SHA2();
                default:
                    throw new ArgumentException($"Invalid value for {nameof(mode)}");
            }
        }
    }
}
