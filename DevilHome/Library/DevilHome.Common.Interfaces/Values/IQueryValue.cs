using DevilHome.Common.Interfaces.Enums;

namespace DevilHome.Common.Interfaces.Values
{
    public interface IQueryValue
    {
        QueryType QueryType { get; set; }
        RequestType RequestType { get; set; }
        FunctionType FunctionType { get; set; }
        string Action { get; set; }
    }
}