using ACVPCore.Models;

namespace Web.Public.Models
{
    public class Vendor
    {
        public string Name { get; set; }
        public string Website { get; set; }
        public string[] Emails { get; set; }
        
        public PersonPhone[] PhoneNumbers { get; set; }
        public Address[] Addresses { get; set; }
    }
}