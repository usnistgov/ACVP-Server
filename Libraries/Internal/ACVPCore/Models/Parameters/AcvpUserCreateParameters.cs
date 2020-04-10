using System;
using System.Collections.Generic;
using System.Text;

namespace ACVPCore.Models.Parameters
{
    public class AcvpUserCreateParameters
    {
        public Person Person { get; set; }
        public byte[] Certficate { get; set; }
        public string Seed { get; set; }
    }
}
