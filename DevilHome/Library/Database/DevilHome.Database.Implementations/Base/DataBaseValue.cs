using System;
using DevilHome.Database.Interfaces.Base;
using SQLite.Net.Attributes;

namespace DevilHome.Database.Implementations.Base
{
    public class DatabaseValue : IDatabaseValue
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Date { get; set; } = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
    }
}