using System;
using DevilHome.Common.Interfaces.Base;

namespace DevilHome.Common.Implementations.Base
{
    public class BaseValue : IBaseValue
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}