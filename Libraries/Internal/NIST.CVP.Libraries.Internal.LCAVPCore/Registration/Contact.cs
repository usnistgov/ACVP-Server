using System.Collections.Generic;
using Newtonsoft.Json;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.Registration
{
	public class Contact
	{
		[JsonIgnore]
		public int PersonID { get; set; }

		[JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
		public string Url { get { return PersonID == 0 ? null : $"/admin/validations/persons/{PersonID}"; } }

		[JsonProperty("fullName", NullValueHandling = NullValueHandling.Ignore)]
		public string Name { get; set; }

		[JsonProperty("emails")]
		public List<string> Emails { get; set; } = new List<string>();

		[JsonProperty("phoneNumbers", NullValueHandling = NullValueHandling.Ignore)]
		public List<PhoneNumber> PhoneNumbers { get; set; } = new List<PhoneNumber>();

		[JsonIgnore]
		public long? OrganizationID { get; set; }

		[JsonProperty("organizationUrl", NullValueHandling = NullValueHandling.Ignore)]
		public string OrganizationUrl { get => OrganizationID == null ? null : $"/admin/validations/organizations/{OrganizationID}"; }

		public bool ShouldSerializePhoneNumbers()
		{
			return PhoneNumbers.Count > 0;
		}

		//Added as a workaround for disjoint between the old logic and the new logic when it came to deleting phone numbers
		[JsonIgnore]
		public bool PhoneNumbersUpdated { get; set; } = false;
	}
}
