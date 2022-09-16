using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Generation.Core.JsonConverters
{
    public class EnumStringConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            Enum e = (Enum)value;
            var description = EnumHelpers.GetEnumDescriptionFromEnum(e);

            if (!String.IsNullOrEmpty(description))
            {
                writer.WriteValue(description);
            }
            else
            {
                // Enum has no description, throw an error
                throw new JsonException($"No description available for enum of type {e.GetType()}");
                // writer.WriteValue(value);
            }
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                throw new JsonException("Unable to parse null enum");
            }

            string? enumText;
            try
            {
                if (reader.TokenType == JsonToken.String)
                {
                    enumText = reader.Value?.ToString();
                    if (String.IsNullOrEmpty(enumText))
                    {
                        throw new JsonException("Unable to parse null or empty string enum");
                    }
                }
                else
                {
                    throw new JsonException("Unable to parse non-string enum value");
                }

            }
            catch (Exception ex)
            {
                throw new JsonException($"Unexpected error parsing JSON enum of type {objectType}", ex);
            }
            
            try
            {
                return objectType
                        .GetFields()
                        .First(
                            f => f.GetCustomAttributes<EnumMemberAttribute>()
                                .Any(a => a.Value.Equals(enumText, StringComparison.Ordinal))   // CASE SENSITIVE FORCED
                        )
                        .GetValue(null);
            }
            catch (Exception ex)
            {
                throw new JsonException($"Unexpected error parsing JSON enum of type {objectType}", ex);
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.IsEnum && !objectType.IsDefined(typeof(FlagsAttribute), false);
        }
    }
}
