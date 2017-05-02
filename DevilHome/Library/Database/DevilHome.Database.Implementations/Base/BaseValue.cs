using DevilHome.Database.Interfaces.Base;
using SQLite.Net.Attributes;

namespace DevilHome.Database.Implementations.Base
{
    public class BaseValue : IBaseValue
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
    }
}