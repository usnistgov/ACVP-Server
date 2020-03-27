using System;
using System.Collections.Generic;
using System.Text;

namespace ACVPCore.Models
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
