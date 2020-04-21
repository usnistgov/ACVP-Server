using System;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Models
{
    public class TestSessionLite
    {
        public long TestSessionId { get; set; }
        public DateTime Created { get; set; }
        public TestSessionStatus Status { get; set; }
    }
}