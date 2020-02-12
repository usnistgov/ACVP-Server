using System;
using System.Collections.Generic;
using System.Text;

namespace ACVPCore.Models
{
    public class Implementation
    {
        public long ID { get; set; }
        public Organization Vendor { get; set; }
        public Address Address { get; set; }
        public string URL { get; set; }
        public string Name { get; set; }
        public enum ModuleType { 
            FIRMWARE = 2,
            HARDWARE = 1,
            SOFTWARE = 0,
            UNKNOWN = 3
        }
        public ModuleType Type { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public bool ITAR { get; set; }
    }
}
