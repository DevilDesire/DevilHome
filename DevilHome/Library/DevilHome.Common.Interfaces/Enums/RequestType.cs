using DevilHome.Common.Interfaces.Extensions;

namespace DevilHome.Common.Interfaces.Enums
{
    public enum RequestType
    {
        [StringValue("get")]
        Get,
        [StringValue("set")]
        Set
    }
}