﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ACVPCore.Models
{
    public class Organization
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string VoiceNumber { get; set; }
        public string FaxNumber { get; set; }
        public Organization Parent { get; set; }
    }
}