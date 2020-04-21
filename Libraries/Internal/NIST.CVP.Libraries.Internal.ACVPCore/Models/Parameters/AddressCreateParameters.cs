namespace NIST.CVP.Libraries.Internal.ACVPCore.Models.Parameters
{
	public class AddressCreateParameters
	{
		public long OrganizationID { get; set; }
		public int OrderIndex { get; set; }
		public string Street1 { get; set; }
		public string Street2 { get; set; }
		public string Street3 { get; set; }
		public string Locality { get; set; }
		public string Region { get; set; }
		public string Country { get; set; }
		public string PostalCode { get; set; }
	}
}
