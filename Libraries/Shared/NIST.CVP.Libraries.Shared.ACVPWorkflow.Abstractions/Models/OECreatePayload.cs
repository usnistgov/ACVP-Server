using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;

namespace NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Models
{
	public class OECreatePayload : BasePayload, IWorkflowItemPayload
	{
		[JsonPropertyName("id")]
		public long ID { get => -1; }

		[JsonPropertyName("url")]
		public string URL { get => "/admin/oes/-1"; }

		[JsonPropertyName("type")]
		public string Type { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("description")]
		public string Description { get; set; }

		//Bring these back when we make the message payload look like what gets sent to us
		//[JsonPropertyName("dependencyUrls")]
		//public List<string> DependencyURLs { get; set; }

		//[JsonPropertyName("dependencies")]			//When we rewrite Public, and pass along the original message, will be able to deserialize directly to this
		//public List<DependencyCreatePayload> DependenciesToCreate { get; set; }

		//This is a temporary, not for deserialization, property that will go away when we change the messages and can go back to the above
		public List<DependencyCreatePayload> DependenciesToCreate => Dependencies?.Where(x => x.IsInlineCreate).Select(x => new DependencyCreatePayload
		{
			Type = x.Type,
			Name = x.Name,
			Description = x.Description,
			Attributes = x.Attributes
		}).ToList();

		[JsonPropertyName("dependencies")]
		public List<Dependency> Dependencies { get; set; }

		public OECreateParameters ToOECreateParameters() => new OECreateParameters
		{
			Name = Name,
			//DependencyIDs = DependencyURLs?.ConvertAll<long>(x => ParseIDFromURL(x)),		//return to this
			DependencyIDs = Dependencies?.Where(x => !x.IsInlineCreate).Select(x => x.ID).ToList()
		};

		//This exists only to deal with the way the messages are implemented in Java, will go away when we rewrite
		public class Dependency
		{
			public bool IsInlineCreate => ID == -1;

			[JsonPropertyName("id")]
			public long ID { get; set; }

			[JsonPropertyName("url")]
			public string URL { get; set; }

			[JsonPropertyName("type")]
			public string Type { get; set; }

			[JsonPropertyName("name")]
			public string Name { get; set; }

			[JsonPropertyName("description")]
			public string Description { get; set; }

			//Since the dependency attributes do not have standard names, and are just key/value pair items, use the JsonExtensionData thing to capture all of the attribute data
			[JsonExtensionData]
			public Dictionary<string, JsonElement> Attributes { get; set; }
		}
	}
}
