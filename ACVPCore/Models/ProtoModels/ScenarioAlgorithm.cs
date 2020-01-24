using System;
using System.Collections.Generic;
using System.Text;
using ACVPCore.Models.Capabilities;

namespace ACVPCore.Models.ProtoModels
{
	public class ScenarioAlgorithm
	{
		public long AlgorithmID { get; set; }

		public List<ICapability> Capabilities { get; set; } = new List<ICapability>();

		public List<string> PrerequisitesNeedToBeAnObject { get; set; }
	}
}
