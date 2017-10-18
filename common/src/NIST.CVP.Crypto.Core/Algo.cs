using System;
using System.ComponentModel;

namespace NIST.CVP.Crypto.Core
{
    public enum Algo
    {
        [Description("TDES-CFB1")]
        TDES_CFB1,
        [Description("TDES-CFB8")]
        TDES_CFB8,
        [Description("TDES-CFB64")]
        TDES_CFB64,
        [Description("AES-CFB1")]
        AES_CFB1,
        [Description("AES-CFB8")]
        AES_CFB8,
        [Description("AES-CFB128")]
        AES_CFB128,
    }

    public static class EnumEx
    {
        public static T FromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute &&
                    attribute.Description == description)
                {
                    return (T)field.GetValue(null);
                }
            }
            throw new InvalidEnumArgumentException($"Unable to return a {typeof(T)} from {description}");
        }
    }
}