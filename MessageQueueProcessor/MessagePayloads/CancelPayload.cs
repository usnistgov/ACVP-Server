﻿using System.Text.Json.Serialization;

namespace MessageQueueProcessor.MessagePayloads
{
	public class CancelPayload
	{
		[JsonPropertyName("tsId")]
		public long TestSessionID { get; set; }
		[JsonPropertyName("vsId")]
		public long VectorSetID { get; set; }
	}
}