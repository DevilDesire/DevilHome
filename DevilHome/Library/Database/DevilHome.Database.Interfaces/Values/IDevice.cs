﻿using DevilHome.Database.Interfaces.Base;

namespace DevilHome.Database.Interfaces.Values
{
    public interface IDevice : IBaseValue
    {
        int Fk_Devicegroup_Id { get; set; }
    }
}