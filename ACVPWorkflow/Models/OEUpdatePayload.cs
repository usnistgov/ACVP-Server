using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ACVPWorkflow.Models
{
	public class OEUpdatePayload
	{
		private string _name;
		private List<string> _dependencyUrls;

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

		[JsonPropertyName("dependencyUrls")]
		public List<string> DependencyURLs
	{
			get => _dependencyUrls;
			set
			{
				_dependencyUrls = value;
				DependenciesUpdated = true;
			}
		}

		public bool NameUpdated { get; private set; } = false;
		public bool DependenciesUpdated { get; private set; } = false;
	}
}
