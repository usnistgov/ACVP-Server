using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ACVPCore.Algorithms.DataTypes
{
	public class Domain
	{
		public List<IDomainSegment> Segments { get; set; } = new List<IDomainSegment>();
	}

	public class DomainConverter : JsonConverter<Domain>
	{
		public override Domain Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			//Create the object to eventually return
			Domain output = new Domain();

			//Get the collection of JsonElements in the array, which we need to then parse individually
			List<JsonElement> unparsedSegments = JsonSerializer.Deserialize<List<JsonElement>>(ref reader);

			//Parse/deserialize each of the items in the array
			foreach (JsonElement segment in unparsedSegments)
			{
				//First try to parse it as a number
				if (long.TryParse(segment.ToString(), out long value))
				{
					output.Segments.Add(new NumericSegment { Value = value });
				}
				else
				{
					//If not a number, it should be a range. But always possible it really isn't, so deserialize it, then see if there's a reasonable Range leftover
					Range range = JsonSerializer.Deserialize<Range>(segment.ToString());

					//The deserialization will always work, but did it yield a useful range? Only add the segment if useful
					if (range.Min != null || range.Max != null)
					{
						output.Segments.Add(range);
					}
				}
			}

			return output;
		}

		public override void Write(Utf8JsonWriter writer, Domain value, JsonSerializerOptions options)
		{
			//Not implementing this because not using it, yet
			throw new NotImplementedException();
		}
	}
}
