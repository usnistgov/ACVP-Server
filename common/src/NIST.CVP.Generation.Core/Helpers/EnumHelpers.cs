using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NIST.CVP.Generation.Core.Helpers
{
    public static class EnumHelpers
    {
        public static string GetEnumDescriptionFromEnum(Enum enumToGetDescriptionFrom)
        {
            FieldInfo fi = enumToGetDescriptionFrom.GetType().GetField(enumToGetDescriptionFrom.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                    typeof(DescriptionAttribute),
                    false);

            if (attributes != null &&
                attributes.Length > 0)
            {
                return attributes[0].Description;
            }

            return enumToGetDescriptionFrom.ToString();
        }

        public static T GetEnumFromEnumDescription<T>(string enumDescription)
            where T : struct
        {
            if (!typeof(T).IsEnum)
            {
                throw new InvalidOperationException("Type is not an enum");
            }

            return (T)typeof(T)
                .GetFields()
                .First(
                    f => f.GetCustomAttributes<DescriptionAttribute>()
                    .Any(a => a.Description.Equals(enumDescription, StringComparison.OrdinalIgnoreCase))
                )
                .GetValue(null);
        }
    }
}
