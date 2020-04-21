using System.Collections.Generic;

namespace NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models
{
    public class Person
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public Organization Organization { get; set; }
        public List<PersonPhone> PhoneNumbers { get; set; }
        public List<string> EmailAddresses { get; set; }
        public List<PersonNote> Notes { get; set; }
    }
}
