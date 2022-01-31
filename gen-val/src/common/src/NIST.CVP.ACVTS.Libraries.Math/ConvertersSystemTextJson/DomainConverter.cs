using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Math.ConvertersSystemTextJson
{
    public class DomainConverter : JsonConverter<MathDomain>
    {
        private readonly IRandom800_90 _random = new Random800_90();

        private class RangeDomainModel
        {
            [JsonPropertyName("min")]
            public int Min { get; set; }
            [JsonPropertyName("max")]
            public int Max { get; set; }
            [JsonPropertyName("increment")]
            public int Increment { get; set; } = 1;
        }

        public override MathDomain Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            //Create the object to eventually return
            MathDomain output = new MathDomain();

            //Get the collection of JsonElements in the array, which we need to then parse individually
            List<JsonElement> unparsedSegments = JsonSerializer.Deserialize<List<JsonElement>>(ref reader);

            if (unparsedSegments == null)
                throw new JsonException($"Expected a non empty array of domain objects.");

            //Parse/deserialize each of the items in the array
            foreach (JsonElement segment in unparsedSegments)
            {
                //First try to parse it as a number
                if (int.TryParse(segment.ToString(), out var value))
                {
                    output.AddSegment(new ValueDomainSegment(value));
                }
                else
                {
                    var segmentJson = segment.ToString();

                    if (string.IsNullOrEmpty(segmentJson))
                        throw new JsonException($"Expected a range object, got '{segment.GetRawText()}'.");

                    //If not a number, it should be a range. But always possible it really isn't, so deserialize it, then see if there's a reasonable Range leftover
                    var range = JsonSerializer.Deserialize<RangeDomainModel>(segmentJson);

                    if (range == null)
                        throw new JsonException($"Unable to parse '{segment.GetRawText()}'");

                    output.AddSegment(new RangeDomainSegment(_random, range.Min, range.Max, range.Increment));
                }
            }

            return output;
        }

        public override void Write(Utf8JsonWriter writer, MathDomain value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();

            foreach (var segment in value.DomainSegments)
            {
                switch (segment)
                {
                    case ValueDomainSegment valueSegment:
                        writer.WriteNumberValue(valueSegment.RangeMinMax.Minimum);
                        break;
                    case RangeDomainSegment rangeSegment:
                        {
                            var minMax = rangeSegment.RangeMinMax;

                            writer.WriteStartObject();
                            writer.WritePropertyName("min");
                            writer.WriteNumberValue(minMax.Minimum);
                            writer.WritePropertyName("max");
                            writer.WriteNumberValue(minMax.Maximum);
                            writer.WritePropertyName("increment");
                            writer.WriteNumberValue(minMax.Increment);
                            writer.WriteEndObject();
                            break;
                        }
                }
            }

            writer.WriteEndArray();
        }
    }
}
