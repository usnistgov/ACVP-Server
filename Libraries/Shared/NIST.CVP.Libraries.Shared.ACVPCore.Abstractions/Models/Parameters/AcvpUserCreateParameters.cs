using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Models.Parameters
{
    public class AcvpUserCreateParameters
    {
        public PersonCreateParameters Person { get; set; }
        public byte[] Certificate { get; set; }
    }
}
