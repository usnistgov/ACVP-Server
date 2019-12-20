using System;
using System.Collections.Generic;
using System.Text;

namespace ACVPCore.Models
{
	public class Address
	{
		public long ID { get; set; }
		public string Street1 { get; set; }
		public string Street2 { get; set; }
		public string Street3 { get; set; }
		public string Locality { get; set; }
		public string Region { get; set; }
		public string Country { get; set; }
		public string PostalCode { get; set; }
	}
}
