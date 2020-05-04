using System.Text.Json.Serialization;

namespace Web.Public.JsonObjects
{
	public class VectorSetPostAnswersObject
	{
		private readonly long _tsId;
		private readonly long _vsId;

		public VectorSetPostAnswersObject(long tsId, long vsId)
		{
			_tsId = tsId;
			_vsId = vsId;
		}

		[JsonPropertyName("url")] public string Url => $"/acvp/v1/testSessions/{_tsId}/vectorSets/{_vsId}";
	}
}