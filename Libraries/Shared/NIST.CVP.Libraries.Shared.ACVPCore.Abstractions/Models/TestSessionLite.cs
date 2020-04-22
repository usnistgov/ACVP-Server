using System;

namespace NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models
{
    public class TestSessionLite
    {
        public long TestSessionId { get; set; }
        public DateTime Created { get; set; }
        public TestSessionStatus Status { get; set; }
    }
}