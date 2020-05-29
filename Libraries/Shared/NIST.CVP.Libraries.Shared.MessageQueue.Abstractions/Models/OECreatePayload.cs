using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;

namespace NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models
{
	public class OECreatePayload : BasePayload, IWorkflowItemPayload
	{
		[JsonPropertyName("id")]
		public long ID { get => -1; }

		[JsonPropertyName("url")]
		public string URL { get => "/admin/oes/-1"; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("dependencyUrls")]
		public List<string> DependencyURLs { get; set; }

		[JsonPropertyName("dependencies")]
		public List<DependencyCreatePayload> DependenciesToCreate { get; set; }

		public OECreateParameters ToOECreateParameters() => new OECreateParameters
		{
			Name = Name,
			DependencyIDs = DependencyURLs?.ConvertAll<long>(x => ParseIDFromURL(x)) ?? new List<long>()
		};
	}
}
