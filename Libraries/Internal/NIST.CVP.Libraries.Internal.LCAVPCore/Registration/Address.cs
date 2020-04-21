using Newtonsoft.Json;
using System.Collections.Generic;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.Registration
{
	public class Address
	{
		[JsonIgnore]
		public long ID { get; set; }

		[JsonIgnore]
		public long VendorID { get; set; }

		[JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
		public string Url { get { return ID == 0 ? null : $"/admin/validations/organizations/{VendorID}/addresses/{ID}"; } }

		[JsonProperty("street1", NullValueHandling = NullValueHandling.Ignore)]
		public string Street1 { get; set; }

		[JsonProperty("street2", NullValueHandling = NullValueHandling.Ignore)]
		public string Street2 { get; set; }

		[JsonProperty("street3", NullValueHandling = NullValueHandling.Ignore)]
		public string Street3 { get; set; }

		[JsonProperty("locality", NullValueHandling = NullValueHandling.Ignore)]
		public string Locality { get; set; }

		[JsonProperty("region", NullValueHandling = NullValueHandling.Ignore)]
		public string Region { get; set; }

		[JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
		public string Country { get; set; }

		[JsonProperty("postalCode", NullValueHandling = NullValueHandling.Ignore)]
		public string PostalCode { get; set; }


		//Added as a workaround for disjoint between the old logic and the new logic due to nested stuff making it hard to tell what actually changed
		[JsonIgnore]
		public bool Street1Updated { get; set; } = false;

		[JsonIgnore]
		public bool Street2Updated { get; set; } = false;

		[JsonIgnore]
		public bool Street3Updated { get; set; } = false;

		[JsonIgnore]
		public bool LocalityUpdated { get; set; } = false;

		[JsonIgnore]
		public bool RegionUpdated { get; set; } = false;

		[JsonIgnore]
		public bool CountryUpdated { get; set; } = false;

		[JsonIgnore]
		public bool PostalCodeUpdated { get; set; } = false;
	}
}
