using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Generation.Core.Enums;

namespace NIST.CVP.Generation.Core
{
    public class DispositionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Enum);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Don't think this is needed right now
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Disposition disposition = (Disposition)value;
            writer.WriteValue(EnumHelpers.GetEnumDescriptionFromEnum(disposition));
        }
    }
}
