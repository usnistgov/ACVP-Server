using Newtonsoft.Json;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.Registration
{
	public class Prerequisite
	{
		[JsonProperty("requirement")]
		public string Algorithm { get; set; }

		[JsonIgnore]
		public long? ValidationRecordID { get; set; }

		[JsonProperty("validationUrl", NullValueHandling = NullValueHandling.Ignore)]
		public string ValidationURL { get => ValidationRecordID == null ? null : $"/admin/validations/{ValidationRecordID}"; }

		[JsonProperty("submissionId", NullValueHandling = NullValueHandling.Ignore)]
		public string SubmissionID { get; set; }

		[JsonIgnore]
		public bool IsUnprocessedSubmission { get; set; } = false;
	}
}
