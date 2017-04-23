using System;
using DevilHome.Database.Implementations.Base;
using DevilHome.Database.Interfaces.Values;

namespace DevilHome.Database.Implementations.Values
{
    public class TimeControlValue : BaseValue, ITimeControlValue
    {
        public int Fk_Room_Id { get; set; }
        public int Fk_Poweroutlet_Id { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public bool Monday { get; set; }
        public bool Thuesday { get; set; }
        public bool Wednessday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }
        public bool IsActive { get; set; }
    }
}