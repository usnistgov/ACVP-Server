using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;

namespace NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Models
{
	public class OEUpdatePayload : BasePayload, IWorkflowItemPayload
	{
		private string _name;
		private List<string> _dependencyUrls;							//bring back after rewrite
		private List<DependencyCreatePayload> _dependenciesToCreate;	//bring back after rewrite
		
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

		[JsonPropertyName("dependencies")]
		public List<DependencyCreatePayload> DependenciesToCreate
		{
			get => _dependenciesToCreate;
			set
			{
				_dependenciesToCreate = value;
				DependenciesUpdated = true;
			}
		}

		public bool NameUpdated { get; private set; } = false;
		public bool DependenciesUpdated { get; private set; } = false;

		public OEUpdateParameters ToOEUpdateParameters() => new OEUpdateParameters
		{
			ID = ID,
			Name = Name,
			DependencyIDs = DependencyURLs?.ConvertAll<long>(x => ParseIDFromURL(x)),
			NameUpdated = NameUpdated,
			DependenciesUpdated = DependenciesUpdated
		};
	}
}
