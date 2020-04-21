namespace NIST.CVP.Libraries.Internal.ACVPCore.Models
{
	public class Address
	{
		public long ID { get; set; }
		public long OrganizationID { get; set; }
		public string Street1 { get; set; }
		public string Street2 { get; set; }
		public string Street3 { get; set; }
		public string Locality { get; set; }
		public string Region { get; set; }
		public string Country { get; set; }
		public string PostalCode { get; set; }
	}
}
