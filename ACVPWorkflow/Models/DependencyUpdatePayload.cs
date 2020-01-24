using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using ACVPCore.Models.Parameters;

namespace ACVPWorkflow.Models
{
	public class DependencyUpdatePayload
	{
		private string _type;
		private string _name;
		private string _description;
		private Dictionary<string, JsonElement> _attributes;

		[JsonPropertyName("id")]
		public long ID { get; set; }

		[JsonPropertyName("url")]
		public string URL { get => $"/admin/dependencies/{ID}"; }

		[JsonPropertyName("type")]
		public string Type
		{
			get => _type;
			set
			{
				_type = value;
				TypeUpdated = true;
			}
		}

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

		[JsonPropertyName("description")]
		public string Description
		{
			get => _description;
			set
			{
				_description = value;
				DescriptionUpdated = true;
			}
		}

		//Since the dependency attributes do not have standard names, and are just key/value pair items, use the JsonExtensionData thing to capture all of the attribute data
		[JsonExtensionData]
		public Dictionary<string, JsonElement> ExtensionDataAttributes
		{
			get => _attributes;
			set
			{
				_attributes = value;
				AttributesUpdated = true;
			}
		}

		////But use this to turn the attributes into something less goofy	
		//public List<DependencyAttributeCreateParameters> Attributes =>
		//	//If no extension data (attributes) then return an empty collection. If there are attributes, return a collection of property objects
		//	ExtensionDataAttributes == null ? new List<DependencyAttributeCreateParameters>() : ExtensionDataAttributes.Select(a => new DependencyAttributeCreateParameters { Name = a.Key, Value = a.Value.GetString() }).ToList();



		//[JsonIgnore]
		public bool TypeUpdated { get; private set; } = false;

		//[JsonIgnore]
		public bool NameUpdated { get; private set; } = false;

		//[JsonIgnore]
		public bool DescriptionUpdated { get; private set; } = false;

		//[JsonIgnore]
		public bool AttributesUpdated { get; private set; } = false;


		public DependencyUpdateParameters ToDependencyUpdateParameters() =>
			new DependencyUpdateParameters
			{
				ID = ID,
				Type = Type,
				Name = Name,
				Description = Description,
				Attributes = GetAttributeParameters(),
				TypeUpdated = TypeUpdated,
				NameUpdated = NameUpdated,
				DescriptionUpdated = DescriptionUpdated,
				AttributesUpdated = AttributesUpdated
			};

		private List<DependencyAttributeCreateParameters> GetAttributeParameters() =>
			//If no extension data (attributes) then return an empty collection. If there are attributes, return a collection of property objects
			ExtensionDataAttributes == null ? new List<DependencyAttributeCreateParameters>() : ExtensionDataAttributes.Select(a => new DependencyAttributeCreateParameters { Name = a.Key, Value = a.Value.GetString() }).ToList();
	}
}
