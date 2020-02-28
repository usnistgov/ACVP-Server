using Newtonsoft.Json;

namespace LCAVPCore.Registration
{
	public class Prerequisite
	{
		[JsonProperty("requirement")]
		public string Algorithm { get; set; }

		[JsonIgnore]
		public int? ValidationRecordID { get; set; }

		[JsonProperty("validationUrl", NullValueHandling = NullValueHandling.Ignore)]
		public string ValidationURL { get => ValidationRecordID == null ? null : $"/admin/validations/{ValidationRecordID}"; }

		[JsonProperty("submissionId", NullValueHandling = NullValueHandling.Ignore)]
		public string SubmissionID { get; set; }
	}
}
