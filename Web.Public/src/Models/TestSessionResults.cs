using System.Collections.Generic;
using System.Text.Json.Serialization;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;

namespace Web.Public.Models
{
	public class TestSessionResults
	{
		[JsonIgnore]
		public TestSessionStatus Status { get; set; }
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
		public string StatusString => EnumHelpers.GetEnumDescriptionFromEnum(Status);

		public VectorSetResultsForTestSession(long tsId, long vsId, VectorSetStatus status)
		{
			TestSessionId = tsId;
			VectorSetId = vsId;
			Status = status;
		}
	}
}