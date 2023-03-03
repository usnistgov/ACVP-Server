using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Helpers
{
    public static class AttributesHelper
    {
        public static readonly Dictionary<LmOtsMode, LmOtsAttribute> LmotsAttributes =
            new()
            {
                {
                    LmOtsMode.LMOTS_SHA256_N24_W1,
                    new LmOtsAttribute(
                    LmOtsMode.LMOTS_SHA256_N24_W1, 0x00000005.GetBytes(), 24, 1, 200, 192, 8, 8, ModeValues.SHA2)
                },
                {
                    LmOtsMode.LMOTS_SHA256_N24_W2,
                    new LmOtsAttribute(
                    LmOtsMode.LMOTS_SHA256_N24_W2, 0x00000006.GetBytes(), 24, 2, 101, 96, 5, 6, ModeValues.SHA2)
                },
                {
                    LmOtsMode.LMOTS_SHA256_N24_W4,
                    new LmOtsAttribute(
                    LmOtsMode.LMOTS_SHA256_N24_W4, 0x00000007.GetBytes(), 24, 4, 51, 48, 3, 4, ModeValues.SHA2)
                },
                {
                    LmOtsMode.LMOTS_SHA256_N24_W8,
                    new LmOtsAttribute(
                    LmOtsMode.LMOTS_SHA256_N24_W8, 0x00000008.GetBytes(), 24, 8, 26, 24, 2, 0, ModeValues.SHA2)
                },


                {
                    LmOtsMode.LMOTS_SHA256_N32_W1,
                    new LmOtsAttribute(
                    LmOtsMode.LMOTS_SHA256_N32_W1, 0x00000001.GetBytes(), 32, 1, 265, 256, 9, 7, ModeValues.SHA2)
                },
                {
                    LmOtsMode.LMOTS_SHA256_N32_W2,
                    new LmOtsAttribute(
                    LmOtsMode.LMOTS_SHA256_N32_W2, 0x00000002.GetBytes(), 32, 2, 133, 128, 5, 6, ModeValues.SHA2)
                },
                {
                    LmOtsMode.LMOTS_SHA256_N32_W4,
                    new LmOtsAttribute(
                    LmOtsMode.LMOTS_SHA256_N32_W4, 0x00000003.GetBytes(), 32, 4, 67, 64, 3, 4, ModeValues.SHA2)
                },
                {
                    LmOtsMode.LMOTS_SHA256_N32_W8,
                    new LmOtsAttribute(
                    LmOtsMode.LMOTS_SHA256_N32_W8, 0x00000004.GetBytes(), 32, 8, 34, 32, 2, 0, ModeValues.SHA2)
                },


                {
                    LmOtsMode.LMOTS_SHAKE_N24_W1,
                    new LmOtsAttribute(
                    LmOtsMode.LMOTS_SHAKE_N24_W1, 0x0000000D.GetBytes(), 24, 1, 200, 192, 8, 8, ModeValues.SHAKE)
                },
                {
                    LmOtsMode.LMOTS_SHAKE_N24_W2,
                    new LmOtsAttribute(
                    LmOtsMode.LMOTS_SHAKE_N24_W2, 0x0000000E.GetBytes(), 24, 2, 101, 96, 5, 6, ModeValues.SHAKE)
                },
                {
                    LmOtsMode.LMOTS_SHAKE_N24_W4,
                    new LmOtsAttribute(
                    LmOtsMode.LMOTS_SHAKE_N24_W4, 0x0000000F.GetBytes(), 24, 4, 51, 48, 3, 4, ModeValues.SHAKE)
                },
                {
                    LmOtsMode.LMOTS_SHAKE_N24_W8,
                    new LmOtsAttribute(
                    LmOtsMode.LMOTS_SHAKE_N24_W8, 0x00000010.GetBytes(), 24, 8, 26, 24, 2, 0, ModeValues.SHAKE)
                },


                {
                    LmOtsMode.LMOTS_SHAKE_N32_W1,
                    new LmOtsAttribute(
                    LmOtsMode.LMOTS_SHAKE_N32_W1, 0x00000009.GetBytes(), 32, 1, 265, 256, 9, 7, ModeValues.SHAKE)
                },
                {
                    LmOtsMode.LMOTS_SHAKE_N32_W2,
                    new LmOtsAttribute(
                    LmOtsMode.LMOTS_SHAKE_N32_W2, 0x0000000A.GetBytes(), 32, 2, 133, 128, 5, 6, ModeValues.SHAKE)
                },
                {
                    LmOtsMode.LMOTS_SHAKE_N32_W4,
                    new LmOtsAttribute(
                    LmOtsMode.LMOTS_SHAKE_N32_W4, 0x0000000B.GetBytes(), 32, 4, 67, 64, 3, 4, ModeValues.SHAKE)
                },
                {
                    LmOtsMode.LMOTS_SHAKE_N32_W8,
                    new LmOtsAttribute(
                    LmOtsMode.LMOTS_SHAKE_N32_W8, 0x0000000C.GetBytes(), 32, 8, 34, 32, 2, 0, ModeValues.SHAKE)
                },
            };

        public static readonly Dictionary<LmsMode, LmsAttribute> LmsAttributes = new()
        {
            {
                LmsMode.LMS_SHA256_M24_H5,
                new LmsAttribute(
                LmsMode.LMS_SHA256_M24_H5, 0x0000000A.GetBytes(), 24, 5, ModeValues.SHA2)
            },
            {
                LmsMode.LMS_SHA256_M24_H10,
                new LmsAttribute(
                LmsMode.LMS_SHA256_M24_H10, 0x0000000B.GetBytes(), 24, 10, ModeValues.SHA2)
            },
            {
                LmsMode.LMS_SHA256_M24_H15,
                new LmsAttribute(
                LmsMode.LMS_SHA256_M24_H15, 0x0000000C.GetBytes(), 24, 15, ModeValues.SHA2)
            },
            {
                LmsMode.LMS_SHA256_M24_H20,
                new LmsAttribute(
                LmsMode.LMS_SHA256_M24_H20, 0x0000000D.GetBytes(), 24, 20, ModeValues.SHA2)
            },
            {
                LmsMode.LMS_SHA256_M24_H25,
                new LmsAttribute(
                LmsMode.LMS_SHA256_M24_H25, 0x0000000E.GetBytes(), 24, 25, ModeValues.SHA2)
            },

            {
                LmsMode.LMS_SHA256_M32_H5,
                new LmsAttribute(
                LmsMode.LMS_SHA256_M32_H5, 0x00000005.GetBytes(), 32, 5, ModeValues.SHA2)
            },
            {
                LmsMode.LMS_SHA256_M32_H10,
                new LmsAttribute(
                LmsMode.LMS_SHA256_M32_H10, 0x00000006.GetBytes(), 32, 10, ModeValues.SHA2)
            },
            {
                LmsMode.LMS_SHA256_M32_H15,
                new LmsAttribute(
                LmsMode.LMS_SHA256_M32_H15, 0x00000007.GetBytes(), 32, 15, ModeValues.SHA2)
            },
            {
                LmsMode.LMS_SHA256_M32_H20,
                new LmsAttribute(
                LmsMode.LMS_SHA256_M32_H20, 0x00000008.GetBytes(), 32, 20, ModeValues.SHA2)
            },
            {
                LmsMode.LMS_SHA256_M32_H25,
                new LmsAttribute(
                LmsMode.LMS_SHA256_M32_H25, 0x00000009.GetBytes(), 32, 25, ModeValues.SHA2)
            },

            {
                LmsMode.LMS_SHAKE_M24_H5,
                new LmsAttribute(
                LmsMode.LMS_SHAKE_M24_H5, 0x00000014.GetBytes(), 24, 5, ModeValues.SHAKE)
            },
            {
                LmsMode.LMS_SHAKE_M24_H10,
                new LmsAttribute(
                LmsMode.LMS_SHAKE_M24_H10, 0x00000015.GetBytes(), 24, 10, ModeValues.SHAKE)
            },
            {
                LmsMode.LMS_SHAKE_M24_H15,
                new LmsAttribute(
                LmsMode.LMS_SHAKE_M24_H15, 0x00000016.GetBytes(), 24, 15, ModeValues.SHAKE)
            },
            {
                LmsMode.LMS_SHAKE_M24_H20,
                new LmsAttribute(
                LmsMode.LMS_SHAKE_M24_H20, 0x00000017.GetBytes(), 24, 20, ModeValues.SHAKE)
            },
            {
                LmsMode.LMS_SHAKE_M24_H25,
                new LmsAttribute(
                LmsMode.LMS_SHAKE_M24_H25, 0x00000018.GetBytes(), 24, 25, ModeValues.SHAKE)
            },

            {
                LmsMode.LMS_SHAKE_M32_H5,
                new LmsAttribute(
                LmsMode.LMS_SHAKE_M32_H5, 0x0000000F.GetBytes(), 32, 5, ModeValues.SHAKE)
            },
            {
                LmsMode.LMS_SHAKE_M32_H10,
                new LmsAttribute(
                LmsMode.LMS_SHAKE_M32_H10, 0x00000010.GetBytes(), 32, 10, ModeValues.SHAKE)
            },
            {
                LmsMode.LMS_SHAKE_M32_H15,
                new LmsAttribute(
                LmsMode.LMS_SHAKE_M32_H15, 0x00000011.GetBytes(), 32, 15, ModeValues.SHAKE)
            },
            {
                LmsMode.LMS_SHAKE_M32_H20,
                new LmsAttribute(
                LmsMode.LMS_SHAKE_M32_H20, 0x00000012.GetBytes(), 32, 20, ModeValues.SHAKE)
            },
            {
                LmsMode.LMS_SHAKE_M32_H25,
                new LmsAttribute(
                LmsMode.LMS_SHAKE_M32_H25, 0x00000013.GetBytes(), 32, 25, ModeValues.SHAKE)
            },
        };

        /// <summary>
        /// Get the LM-OTS construction attributes based on an <see cref="LmOtsMode"/>.
        /// </summary>
        /// <param name="lmOtsMode">The mode for LM-OTS construction.</param>
        /// <returns><see cref="LmotsAttributes"/>.</returns>
        /// <exception cref="ArgumentException">Thrown when the <see cref="lmOtsMode"/> can't be mapped to an attribute.</exception>
        public static LmOtsAttribute GetLmOtsAttribute(LmOtsMode lmOtsMode)
        {
            if (!LmotsAttributes.TryFirst(w => w.Key == lmOtsMode, out var result))
            {
                throw new ArgumentException($"Couldn't map {nameof(lmOtsMode)} for retrieving attributes.");
            }

            return result.Value;
        }

        /// <summary>
        /// Get the LMS construction attributes based on an <see cref="LmsMode"/>.
        /// </summary>
        /// <param name="lmsMode">The mode for LMS construction.</param>
        /// <returns><see cref="LmsAttributes"/>.</returns>
        /// <exception cref="ArgumentException">Thrown when the <see cref="lmsMode"/> can't be mapped to an attribute.</exception>
        public static LmsAttribute GetLmsAttribute(LmsMode lmsMode)
        {
            if (!LmsAttributes.TryFirst(w => w.Key == lmsMode, out var result))
            {
                throw new ArgumentException($"Couldn't map {nameof(lmsMode)} for retrieving attributes.");
            }

            return result.Value;
        }

        /// <summary>
        /// Gets the length in bytes needed for the buffer used in an underlying one way function.
        ///
        /// <see cref="LmOtsMode.LMOTS_SHA256_N24_W1"/> as an example uses only 24 bytes from the total of 32 bytes
        /// available from the output of SHA256, but the buffer needs to be written to in its entirety.  Only the 24
        /// most significant bytes are utilized from the buffer.
        /// </summary>
        /// <param name="mode">The <see cref="LmOtsMode"/> used for determining the output buffer size.</param>
        /// <returns>The length in bytes of the buffer used for the one way function.</returns>
        /// <exception cref="ArgumentOutOfRangeException">thrown when the <see cref="mode"/> cannot be mapped to an <see cref="LmOtsMode"/>.</exception>
        public static int GetBufferByteLengthBasedOnOneWayFunction(LmOtsMode mode)
        {
            /*
			 TODO: seems strange that the shake buffer can't be 24 (get an exception),
			 shake outputs bits are "whatever you want them to be" in length. 
			 */
            switch (mode)
            {
                case LmOtsMode.LMOTS_SHA256_N24_W1:
                case LmOtsMode.LMOTS_SHA256_N24_W2:
                case LmOtsMode.LMOTS_SHA256_N24_W4:
                case LmOtsMode.LMOTS_SHA256_N24_W8:
                    return 32;
                case LmOtsMode.LMOTS_SHA256_N32_W1:
                case LmOtsMode.LMOTS_SHA256_N32_W2:
                case LmOtsMode.LMOTS_SHA256_N32_W4:
                case LmOtsMode.LMOTS_SHA256_N32_W8:
                    return 32;
                case LmOtsMode.LMOTS_SHAKE_N24_W1:
                case LmOtsMode.LMOTS_SHAKE_N24_W2:
                case LmOtsMode.LMOTS_SHAKE_N24_W4:
                case LmOtsMode.LMOTS_SHAKE_N24_W8:
                    return 32;
                case LmOtsMode.LMOTS_SHAKE_N32_W1:
                case LmOtsMode.LMOTS_SHAKE_N32_W2:
                case LmOtsMode.LMOTS_SHAKE_N32_W4:
                case LmOtsMode.LMOTS_SHAKE_N32_W8:
                    return 32;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

        /// <summary>
        /// Gets the length in bytes needed for the buffer used in an underlying one way function.
        ///
        /// <see cref="LmsMode.LMS_SHA256_M24_H5"/> as an example uses only 24 bytes from the total of 32 bytes
        /// available from the output of SHA256, but the buffer needs to be written to in its entirety.  Only the 24
        /// most significant bytes are utilized from the buffer.
        /// </summary>
        /// <param name="mode">The <see cref="LmsMode"/> used for determining the output buffer size.</param>
        /// <returns>The length in bytes of the buffer used for the one way function.</returns>
        /// <exception cref="ArgumentOutOfRangeException">thrown when the <see cref="mode"/> cannot be mapped to an <see cref="LmsMode"/>.</exception>
        public static int GetBufferByteLengthBasedOnOneWayFunction(LmsMode mode)
        {
            /*
			 TODO: seems strange that the shake buffer can't be 24 (get an exception),
			 shake outputs bits are "whatever you want them to be" in length. 
			 */
            switch (mode)
            {
                case LmsMode.LMS_SHA256_M24_H5:
                case LmsMode.LMS_SHA256_M24_H10:
                case LmsMode.LMS_SHA256_M24_H15:
                case LmsMode.LMS_SHA256_M24_H20:
                case LmsMode.LMS_SHA256_M24_H25:
                    return 32;
                case LmsMode.LMS_SHA256_M32_H5:
                case LmsMode.LMS_SHA256_M32_H10:
                case LmsMode.LMS_SHA256_M32_H15:
                case LmsMode.LMS_SHA256_M32_H20:
                case LmsMode.LMS_SHA256_M32_H25:
                    return 32;
                case LmsMode.LMS_SHAKE_M24_H5:
                case LmsMode.LMS_SHAKE_M24_H10:
                case LmsMode.LMS_SHAKE_M24_H15:
                case LmsMode.LMS_SHAKE_M24_H20:
                case LmsMode.LMS_SHAKE_M24_H25:
                    return 32;
                case LmsMode.LMS_SHAKE_M32_H5:
                case LmsMode.LMS_SHAKE_M32_H10:
                case LmsMode.LMS_SHAKE_M32_H15:
                case LmsMode.LMS_SHAKE_M32_H20:
                case LmsMode.LMS_SHAKE_M32_H25:
                    return 32;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

        /// <summary>
        /// Determines the <see cref="LmOtsMode"/> from a byte array that represents the type code or numeric identifier.
        /// </summary>
        /// <param name="typeCode">The 4 byte typeCode to determine the <see cref="LmOtsMode"/> from.</param>
        /// <returns>The <see cref="LmOtsMode"/>.</returns>
        public static LmOtsMode GetLmOtsModeFromTypeCode(byte[] typeCode)
        {
            if (typeCode == null)
            {
                return LmOtsMode.Invalid;
            }

            return LmotsAttributes
                .FirstOrDefault(w => w.Value.NumericIdentifier.SequenceEqual(typeCode))
                .Key;
        }

        /// <summary>
        /// Determines the <see cref="LmsMode"/> from a byte array that represents the type code or numeric identifier.
        /// </summary>
        /// <param name="typeCode">The 4 byte typeCode to determine the <see cref="LmsMode"/> from.</param>
        /// <returns>The <see cref="LmsMode"/>.</returns>
        public static LmsMode GetLmsModeFromTypeCode(byte[] typeCode)
        {
            if (typeCode == null)
            {
                return LmsMode.Invalid;
            }

            return LmsAttributes
                .FirstOrDefault(w => w.Value.NumericIdentifier.SequenceEqual(typeCode))
                .Key;
        }

        /// <summary>
        /// For the provided <see cref="lmsModes"/> and <see cref="lmOtsModes"/>, return a dictionary keyed on their
        /// output length and underlying hash, returning the applicable <see cref="lmsMode"/> and <see cref="lmOtsModes"/>s
        /// to each combination of output length/hash.
        /// </summary>
        /// <param name="lmsModes">The <see cref="LmsMode"/></param>
        /// <param name="lmOtsModes"></param>
        /// <returns></returns>
        public static List<MappedLmsLmOtsModesToFunctionOutputLength> GetMappedLmsLmOtsModesToFunctionOutputLength(
            IEnumerable<LmsMode> lmsModes, IEnumerable<LmOtsMode> lmOtsModes)
        {
            var result = new List<MappedLmsLmOtsModesToFunctionOutputLength>();

            if (lmsModes == null)
                throw new ArgumentNullException(nameof(lmsModes));
            if (lmOtsModes == null)
                throw new ArgumentNullException(nameof(lmOtsModes));

            var lmsAttributes = lmsModes
                .Select(GetLmsAttribute)
                .GroupBy(gb => new
                {
                    Function = gb.ShaMode,
                    OutputLength = gb.M
                })
                .Select(group => new
                {
                    FunctionOutputLength = group.Key,
                    LmsModes = group.Select(attributes => attributes.Mode).ToList()
                }).ToList();

            var lmOtsAttributes = lmOtsModes
                .Select(GetLmOtsAttribute)
                .GroupBy(gb => new
                {
                    Function = gb.ShaMode,
                    OutputLength = gb.N
                })
                .Select(group => new
                {
                    FunctionOutputLength = group.Key,
                    LmOtsModes = group.Select(attributes => attributes.Mode).ToList()
                }).ToList();

            var functionOutputLengths = lmsAttributes
                .Select(s => new
                {
                    Function = s.FunctionOutputLength.Function,
                    OutputLength = s.FunctionOutputLength.OutputLength
                }).Intersect(lmOtsAttributes.Select(s => new
                {
                    Function = s.FunctionOutputLength.Function,
                    OutputLength = s.FunctionOutputLength.OutputLength
                })).ToList();

            foreach (var functionOutputLength in functionOutputLengths)
            {
                var lmsReturn = new List<LmsMode>();
                var lmOtsReturn = new List<LmOtsMode>();
                result.Add(new MappedLmsLmOtsModesToFunctionOutputLength(
                    functionOutputLength.Function, functionOutputLength.OutputLength, lmsReturn, lmOtsReturn));

                lmsReturn.AddRange(
                    lmsAttributes.Where(w =>
                            w.FunctionOutputLength.OutputLength == functionOutputLength.OutputLength &&
                            w.FunctionOutputLength.Function == functionOutputLength.Function)
                        .SelectMany(sm => sm.LmsModes));
                lmOtsReturn.AddRange(
                    lmOtsAttributes.Where(w =>
                            w.FunctionOutputLength.OutputLength == functionOutputLength.OutputLength &&
                            w.FunctionOutputLength.Function == functionOutputLength.Function)
                        .SelectMany(sm => sm.LmOtsModes));
            }

            return result;
        }
    }
}
