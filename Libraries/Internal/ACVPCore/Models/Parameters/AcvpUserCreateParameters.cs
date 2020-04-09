using System;
using System.Collections.Generic;
using System.Text;

namespace ACVPCore.Models.Parameters
{
    public class AcvpUserCreateParameters
    {
        public PersonCreateParameters Person { get; set; }
        public byte[] Certificate { get; set; }
        public string Seed { get; set; }
    }
}
