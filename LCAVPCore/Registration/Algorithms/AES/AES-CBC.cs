using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ACVPCore.Algorithms.Persisted;
using LCAVPCore.CustomValidators;
using Newtonsoft.Json;

namespace LCAVPCore.Registration.Algorithms.AES
{
	public class AES_CBC : AlgorithmBase, IAlgorithm
	{
		//[CollectionMinLength(1, ErrorMessage = "Direction empty")]
		//[CollectionMaxLength(2, ErrorMessage = "Too many directions")]
		[JsonProperty(PropertyName = "direction")]
		public List<string> Direction { get; set; }

		[Required, MinLength(1, ErrorMessage = "KeyLen empty"), MaxLength(2, ErrorMessage = "Too many KeyLen"), AllowedValues("128, 192, 256", ErrorMessage = "Invalid keylen value")]
		[JsonProperty(PropertyName = "keyLen")]
		public List<int> KeyLen { get; set; }

		public AES_CBC(Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "ACVP-AES-CBC")
		{
			List<string> validValues = new List<string> { "Both", "Encrypt", "Decrypt" };
			KeyLen = new List<int>();
			if (validValues.Contains(options.GetValue("CBC128_State"))) KeyLen.Add(128);
			if (validValues.Contains(options.GetValue("CBC192_State"))) KeyLen.Add(192);
			if (validValues.Contains(options.GetValue("CBC256_State"))) KeyLen.Add(256);

			//Even though the direction is specified for each key length, it can't differ, so don't care how many have the direction value as long as one does
			Direction = new List<string>();
			if (options.ContainsValue("Encrypt") || options.ContainsValue("Both")) Direction.Add("encrypt");
			if (options.ContainsValue("Decrypt") || options.ContainsValue("Both")) Direction.Add("decrypt");
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new ACVPCore.Algorithms.Persisted.AES_CBC
		{
			Direction = Direction,
			KeyLength = KeyLen
		};
	}
}
