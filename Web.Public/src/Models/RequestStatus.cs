using System.Runtime.Serialization;

namespace Web.Public.Models
{
    public enum RequestStatus
    {
        [EnumMember(Value = "initial")]
        Initial,
        [EnumMember(Value = "processing")]
        Processing,
        [EnumMember(Value = "approved")]
        Approved,
        [EnumMember(Value = "rejected")]
        Rejected
    }
}