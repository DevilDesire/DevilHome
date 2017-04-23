namespace DevilHome.Database.Interfaces.Base
{
    public interface IBaseValue
    {
        int Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
    }
}