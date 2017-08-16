using DevilHome.Common.Interfaces.Extensions;

namespace DevilHome.Common.Interfaces.Enums
{
    public enum FunctionType
    {
        [StringValue("default")]
        Default,
        [StringValue("music")]
        Music,
        [StringValue("volume")]
        Volume,
        [StringValue("outlet")]
        Outlet,
        [StringValue("light")]
        Light,
        [StringValue("create")]
        Create,
        [StringValue("edit")]
        Edit,
        [StringValue("delete")]
        Delete
    }
}