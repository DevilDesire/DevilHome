﻿using System.Collections.Generic;
using DevilHome.Common.Implementations.Base;
using DevilHome.Common.Interfaces.Values;

namespace DevilHome.Common.Implementations.Values
{
    public class RoomValue : BaseValue, IRoomValue
    {
        public List<IPoweroutletValue> PoweroutletValues { get; set; } = new List<IPoweroutletValue>();
        public List<ISensorValue> SensorValues { get; set; } = new List<ISensorValue>();
    }
}