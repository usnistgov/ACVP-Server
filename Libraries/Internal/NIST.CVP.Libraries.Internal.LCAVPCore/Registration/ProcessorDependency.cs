using System.Linq;
using Newtonsoft.Json;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.Registration
{
	public class ProcessorDependency : Dependency, IDependency
	{
		private string _name;
		private readonly string[] COMMON_MANUFACTURERS = { "amd", "apple", "arm", "intel", "intel(r)", "intel®", "qualcomm" };

		[JsonProperty("type")]
		public new string Type { get; set; } = "processor";

		[JsonProperty("name")]
		public new string Name
		{
			get => _name; set
			{
				//split the value on space
				var splitName = value.Split(" ".ToCharArray());
				if (COMMON_MANUFACTURERS.Contains(splitName[0].ToLower()))
				{
					//Populate the Manufacturer with the clean value
					Manufacturer = CleanManufacturer(splitName[0]);

					//Replace the potentially dirty value in the split array with the clean value
					splitName[0] = Manufacturer;

					//Join it back together
					_name = string.Join(" ", splitName).Trim();
				}
				else
				{
					_name = value.Trim();
				}
			}
		}

		[JsonProperty("manufacturer", NullValueHandling = NullValueHandling.Ignore)]
		public new string Manufacturer { get; set; }

		[JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
		public new string Description { get => Name; }


		private string CleanManufacturer(string manufacturer)
		{
			switch (manufacturer.ToLower())
			{
				case "arm": return "ARM";
				case "apple": return "Apple";
				case "intel":
				case "intel(r)":
				case "intel®": return "Intel";
				case "qualcomm": return "Qualcomm";
				default: return manufacturer;
			}
		}
	}
}
