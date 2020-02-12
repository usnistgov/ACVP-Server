using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using ACVPCore.Models.Parameters;

namespace ACVPWorkflow.Models
{
	public class OEUpdatePayload : BasePayload, IWorkflowItemPayload
	{
		private string _name;
		//private List<string> _dependencyUrls;							//bring back after rewrite
		//private List<DependencyCreatePayload> _dependenciesToCreate;	//bring back after rewrite
		private List<Dependency> _dependencies;						//kill after rewrite

		[JsonPropertyName("id")]
		public long ID { get; set; }

		[JsonPropertyName("url")]
		public string URL { get => $"/admin/oes/{ID}"; }


		[JsonPropertyName("name")]
		public string Name
		{
			get => _name;
			set
			{
				_name = value;
				NameUpdated = true;
			}
		}

		//Bring these back when we make the message payload look like what gets sent to us
		//[JsonPropertyName("dependencyUrls")]
		//public List<string> DependencyURLs
		//{
		//	get => _dependencyUrls;
		//	set
		//	{
		//		_dependencyUrls = value;
		//		DependenciesUpdated = true;
		//	}
		//}

		//[JsonPropertyName("dependencies")]
		//public List<DependencyCreatePayload> DependenciesToCreate
		//{
		//	get => _dependenciesToCreate;
		//	set
		//	{
		//		_dependenciesToCreate = value;
		//		DependenciesUpdated = true;
		//	}
		//}

		//This is a temporary, not for deserialization, property that will go away when we change the messages and can go back to the above
		public List<DependencyCreatePayload> DependenciesToCreate => Dependencies.Where(x => x.IsInlineCreate).Select(x => new DependencyCreatePayload
		{
			Type = x.Type,
			Name = x.Name,
			Description = x.Description,
			Attributes = x.Attributes
		}).ToList();

		[JsonPropertyName("dependencies")]
		public List<Dependency> Dependencies
		{
			get => _dependencies;
			set
			{
				_dependencies = value;
				DependenciesUpdated = true;
			}
		}

		public bool NameUpdated { get; private set; } = false;
		public bool DependenciesUpdated { get; private set; } = false;

		public OEUpdateParameters ToOEUpdateParameters() => new OEUpdateParameters
		{
			ID = ID,
			Name = Name,
			//DependencyIDs = DependencyURLs.ConvertAll<long>(x => ParseIDFromURL(x)),			//return to this when we rewrite public
			DependencyIDs = Dependencies.Where(x => !x.IsInlineCreate).Select(x => x.ID).ToList(),
			NameUpdated = NameUpdated,
			DependenciesUpdated = DependenciesUpdated
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
