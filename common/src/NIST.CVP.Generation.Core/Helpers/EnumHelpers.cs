using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    }
}
