using System;
using System.Collections.Generic;

namespace NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models
{
    public class Organization
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string VoiceNumber { get; set; }
        public string FaxNumber { get; set; }
        public OrganizationLite Parent { get; set; }
        public List<Address> Addresses { get; set; }
        public List<PersonLite> Persons { get; set; }
        public List<String> Emails { get; set; }
    }
}
