using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace NIST.CVP.ACVTS.Libraries.Generation.Core.JsonConverters
{
    public class FlagEnumConverter : JsonConverter
    {
        /*
         * WriteJson() is a modified copy and paste of Newtonsoft's StringEnumConverter. It should be mostly identical
         * to the Newtonsoft version, except that the Newtonsoft version calls several helper functions whose logic
         * are pasted inline in lieu of re-creating those helper functions.
         */
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            
            Enum e = (Enum)value;

            ulong v = Convert.ToUInt64(value);

            Type enumType = e.GetType();
            string[] names = Enum.GetNames(enumType);
            string[] resolvedNames = new string[names.Length];
            ulong[] values = new ulong[names.Length];
         
            for (int i = 0; i < names.Length; i++)
            {
                string name = names[i];
                FieldInfo f = enumType.GetField(name,
                    BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static)!;
                values[i] = Convert.ToUInt64(f.GetValue(null)!);

                string resolvedName;
                string? specifiedName = f.GetCustomAttributes(typeof(EnumMemberAttribute), true)
                    .Cast<EnumMemberAttribute>()
                    .Select(a => a.Value)
                    .SingleOrDefault();
              
                resolvedName = specifiedName ?? name;

                if (Array.IndexOf(resolvedNames, resolvedName, 0, i) != -1)
                {
                    throw new JsonException("Enum name " + resolvedName + " already exists on enum " +
                                                        enumType.Name);
                }
                resolvedNames[i] = resolvedName;
            }

            string enumName;

            int index = values.Length - 1;
            StringBuilder sb = new StringBuilder();
            bool firstTime = true;
            ulong saveResult = v;

            while (index >= 0)
            {
                if (index == 0 && values[index] == 0)
                {
                    break;
                }

                if ((v & values[index]) == values[index])
                {
                    v -= values[index];
                    if (!firstTime)
                    {
                        sb.Insert(0, ", ");
                    }

                    string resolvedName2 = resolvedNames[index];
                    sb.Insert(0, resolvedName2);
                    firstTime = false;
                }

                index--;
            }

            if (v != 0)
            {
                // We were unable to represent this number as a bitwise or of valid flags
                enumName = null;
            }
            else if (saveResult == 0)
            {
                if (values.Length > 0 && values[0] == 0)
                {
                    enumName = resolvedNames[0]; // Zero was one of the enum values.
                }
                else
                {
                    enumName = null;
                }
            }
            else
            {
                enumName = sb.ToString();
            }
            
            writer.WriteValue(enumName);
        }

        /*
         * ReadJson() is a modified copy and paste of Newtonsoft's StringEnumConverter/ReadJson(). It is mostly
         * identical to the Newtonsoft version, except that:
         * 1) the Newtonsoft version calls several helper functions whose logic are pasted inline in lieu of re-creating
         * those helper functions;
         * 2) Newtonsoft's allowance for case-insensitive matching is removed; and 
         * 3) Newtonsoft's allowance to match, e.g., Enum.None when no [EnumMember(Value = "none")] is specified, is removed
         */
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
                if (!objectType.IsEnum)
                {
                    throw new JsonException($"Unable to parse json to type enum. Type provided is '{nameof(objectType)}', but must be an Enum." );
                }
                
                string[] names = Enum.GetNames(objectType);
                string[] resolvedNames = new string[names.Length];
                ulong[] values = new ulong[names.Length];

                for (int i = 0; i < names.Length; i++)
                {
                    string name = names[i];
                    FieldInfo f = objectType.GetField(name,
                        BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static)!;
                    values[i] = Convert.ToUInt64(f.GetValue(null)!);

                    string resolvedName;
                    string? specifiedName = f.GetCustomAttributes(typeof(EnumMemberAttribute), true)
                        .Cast<EnumMemberAttribute>()
                        .Select(a => a.Value)
                        .SingleOrDefault();
                    resolvedName = specifiedName ?? name;

                    if (Array.IndexOf(resolvedNames, resolvedName, 0, i) != -1)
                    {
                        throw new JsonException("Enum name " + resolvedName + " already exists on enum " +
                                                            objectType.Name);
                    }
                    resolvedNames[i] = resolvedName;
                }
                
                // first check if the entire text (including commas) matches a resolved name
                int? matchingIndex = null;

                for (int i = 0; i < resolvedNames.Length; i++)
                {
                    if (resolvedNames[i].Length == enumText.Length &&
                        string.Compare(resolvedNames[i], 0, enumText, 0, enumText.Length, StringComparison.Ordinal) == 0)
                    {
                        matchingIndex = i;
                    }
                }

                if (matchingIndex != null)
                {
                    return Enum.ToObject(objectType, values[matchingIndex.Value]);
                }
                
                int firstNonWhitespaceIndex = -1;
                for (int i = 0; i < enumText.Length; i++)
                {
                    if (!char.IsWhiteSpace(enumText[i]))
                    {
                        firstNonWhitespaceIndex = i;
                        break;
                    }
                }
                if (firstNonWhitespaceIndex == -1)
                {
                    throw new JsonException("Unable to parse provided enum value to type : " + objectType +
                                                ". Provided enum value contains no non-whitespace values.");
                }
                
                ulong result = 0;

                int enumTextIndex = firstNonWhitespaceIndex;
                while (enumTextIndex <= enumText.Length) // '=' is to handle invalid case of an ending comma
                {
                    // Find the next separator, if there is one, otherwise the end of the string.
                    int endIndex = enumText.IndexOf(',', enumTextIndex);
                    if (endIndex == -1)
                    {
                        endIndex = enumText.Length;
                    }

                    // Shift the starting and ending indices to eliminate whitespace
                    int endIndexNoWhitespace = endIndex;
                    while (enumTextIndex < endIndex && char.IsWhiteSpace(enumText[enumTextIndex]))
                    {
                        enumTextIndex++;
                    }

                    while (endIndexNoWhitespace > enumTextIndex && char.IsWhiteSpace(enumText[endIndexNoWhitespace - 1]))
                    {
                        endIndexNoWhitespace--;
                    }
                    int enumTextSubstringLength = endIndexNoWhitespace - enumTextIndex;

                    // Before we try to find a match, set matchingIndex to null as it will be used in multiple 
                    // iterations of the while loop
                    matchingIndex = null;
                    
                    // match with case sensitivity
                    for (int i = 0; i < resolvedNames.Length; i++)
                    {
                        if (resolvedNames[i].Length == enumTextSubstringLength &&
                            string.Compare(resolvedNames[i], 0, enumText, enumTextIndex, enumTextSubstringLength, StringComparison.Ordinal) == 0)
                        {
                            matchingIndex = i;
                        }
                    }
                    
                    // Newtonsoft/StringEnumConverter does this, but we don't want to
                    // if (matchingIndex == null)
                    // {
                    //     for (int i = 0; i < names.Length; i++)
                    //     {
                    //         if (names[i].Length == enumTextSubstringLength &&
                    //             string.Compare(names[i], 0, enumText, enumTextIndex, enumTextSubstringLength, StringComparison.Ordinal) == 0)
                    //         {
                    //             matchingIndex = i;
                    //         }
                    //     }   
                    // }
                    
                    if (matchingIndex == null)
                    {
                        // no match so error
                        throw new JsonException($"Unable to parse Json. Requested value '{enumText}' was not found.");
                    }

                    result |= values[matchingIndex.Value];

                    // Move our pointer to the ending index to go again.
                    enumTextIndex = endIndex + 1;
                }

                return Enum.ToObject(objectType, result);
                
            }
            catch (Exception ex)
            {
                throw new JsonException($"Unexpected error parsing JSON enum of type {objectType}", ex);
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.IsEnum && objectType.IsDefined(typeof(FlagsAttribute), false);
        }
    }
}
