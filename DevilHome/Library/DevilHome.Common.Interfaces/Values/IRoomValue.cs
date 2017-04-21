using System.Collections.Generic;
using DevilHome.Common.Interfaces.Base;

namespace DevilHome.Common.Interfaces.Values
{
    public interface IRoomValue : IBaseValue
    {
        List<IPoweroutletValue> PoweroutletValues { get; set; }
        List<ISensorValue> SensorValues { get; set; }
    }
}