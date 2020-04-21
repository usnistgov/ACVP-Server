using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;

namespace NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Models
{
	public class PersonCreatePayload : BasePayload, IWorkflowItemPayload
	{
		[JsonPropertyName("id")]
		public long ID { get => -1; }

		[JsonPropertyName("url")]
		public string URL { get => "/admin/persons/-1"; }

		[JsonPropertyName("fullName")]
		public string Name { get; set; }

		[JsonPropertyName("organizationUrl")]
		public string OrganizationURL { get; set; }

		[JsonPropertyName("emails")]
		public List<string> EmailAddresses { get; set; }

		[JsonPropertyName("phoneNumbers")]
		public List<PhoneNumber> PhoneNumbers { get; set; }


		public PersonCreateParameters ToPersonCreateParameters() => new PersonCreateParameters
		{
			Name = Name,
			OrganizationID = ParseIDFromURL(OrganizationURL),
			PhoneNumbers = PhoneNumbers?.OrderBy(x => x.OrderIndex).Select(x => (x.Type, x.Number)).ToList(),
			EmailAddresses = EmailAddresses,
		};


		public class PhoneNumber
		{
			[JsonPropertyName("number")]
			public string Number { get; set; }

			[JsonPropertyName("type")]
			public string Type { get; set; }

			[JsonPropertyName("orderIndex")]
			public int OrderIndex { get; set; }
		}
	}
}
