using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;

namespace Web.Public.Models
{
	public class TestSessionResults
	{
		[JsonIgnore]
		public TestSessionStatus Status { get; set; }
		[JsonPropertyName("createdOn")]
		public DateTime CreatedOn { get; set; }
		[JsonPropertyName("expiresOn")]
		public DateTime ExpiresOn { get; set; }
		[JsonPropertyName("passed")] 
		public bool Passed => Status == TestSessionStatus.Passed;
		[JsonPropertyName("results")]
		public List<VectorSetResultsForTestSession> Type { get; set; }
	}

	public class VectorSetResultsForTestSession
	{
		[JsonIgnore]
		public long TestSessionId { get; }
		[JsonIgnore]
		public long VectorSetId { get; }
		[JsonPropertyName("vectorSetUrl")] 
		public string VectorSetUrl => $"/acvp/v1/testSessions/{TestSessionId}/vectorSets/{VectorSetId}";
		[JsonIgnore]
		public VectorSetStatus Status { get; }
		[JsonPropertyName("status")]
		public string StatusString {
			get
			{
				return Status switch
				{
					VectorSetStatus.Failed => "fail",
					VectorSetStatus.Passed => "passed",
					VectorSetStatus.Initial => "unreceived",
					VectorSetStatus.Processed => "unreceived",
					VectorSetStatus.Error => "error",
					VectorSetStatus.Cancelled => "expired",
					VectorSetStatus.ResubmitAnswers => "incomplete",
					VectorSetStatus.KATReceived => "incomplete",
					_ => "incomplete"
				};
			}}

		public VectorSetResultsForTestSession(long tsId, long vsId, VectorSetStatus status)
		{
			TestSessionId = tsId;
			VectorSetId = vsId;
			Status = status;
		}
	}
}