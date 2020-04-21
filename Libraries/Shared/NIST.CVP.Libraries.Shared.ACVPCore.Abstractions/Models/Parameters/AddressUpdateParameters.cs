namespace NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters
{
	public class AddressUpdateParameters
	{
		public long ID { get; set; }
		public long OrganizationID { get; set; }
		public int OrderIndex { get; set; }
		public string Street1 { get; set; }
		public string Street2 { get; set; }
		public string Street3 { get; set; }
		public string Locality { get; set; }
		public string Region { get; set; }
		public string Country { get; set; }
		public string PostalCode { get; set; }

		public bool Street1Updated { get; set; }
		public bool Street2Updated { get; set; }
		public bool Street3Updated { get; set; }
		public bool LocalityUpdated { get; set; }
		public bool RegionUpdated { get; set; }
		public bool CountryUpdated { get; set; }
		public bool PostalCodeUpdated { get; set; }
	}
}
