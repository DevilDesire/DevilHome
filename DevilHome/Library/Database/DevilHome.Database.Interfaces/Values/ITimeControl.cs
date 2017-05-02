using System;
using DevilHome.Database.Interfaces.Base;

namespace DevilHome.Database.Interfaces.Values
{
    public interface ITimeControl : IBaseValue
    {
        int Fk_Room_Id { get; set; }
        int Fk_Poweroutlet_Id { get; set; }
        DateTime From { get; set; }
        DateTime To { get; set; }
        bool Monday { get; set; }
        bool Thuesday { get; set; }
        bool Wednessday { get; set; }
        bool Thursday { get; set; }
        bool Friday { get; set; }
        bool Saturday { get; set; }
        bool Sunday { get; set; }
        bool IsActive { get; set; }
    }
}