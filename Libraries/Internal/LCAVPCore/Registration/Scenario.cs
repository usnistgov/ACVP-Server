﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace LCAVPCore.Registration
{
	public class Scenario
	{
		[JsonProperty("operatingEnvironments")]
		public List<OperationalEnvironment> OEs { get; set; } = new List<OperationalEnvironment>();

		[JsonProperty("algorithmCapabilities")]
		public List<AlgorithmCapabilities> Algorithms { get; set; } = new List<AlgorithmCapabilities>();
	}
}