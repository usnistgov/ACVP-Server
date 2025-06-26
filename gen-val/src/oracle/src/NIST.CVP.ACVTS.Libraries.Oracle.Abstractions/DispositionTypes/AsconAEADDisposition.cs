using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

public enum AsconAEADDisposition
{
    [EnumMember(Value = "valid input - ciphertext should decrypt successfully")]
    None,

    //[EnumMember(Value = "modified key")]
    //ModifyKey,

    //[EnumMember(Value = "modified nonce")]
    //ModifyNonce,

    //[EnumMember(Value = "modified associated data")]
    //ModifyAD,

    //[EnumMember(Value = "modified ciphertext")]
    //ModifyCiphertext,

    [EnumMember(Value = "modified tag")]
    ModifyTag,

    //[EnumMember(Value = "modified second key")]
    //ModifySecondKey,
}
