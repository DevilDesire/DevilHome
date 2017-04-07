using DevilHome.Common.Interfaces.Enums;
using DevilHome.Common.Interfaces.Values;

namespace DevilHome.Common.Implementations.Values
{
    public class QueryValue : IQueryValue
    {
        public QueryType QueryType { get; set; }
        public RequestType RequestType { get; set; }
        public FunctionType FunctionType { get; set; }
        public string Action { get; set; }
    }
}