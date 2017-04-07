namespace DevilDesireDevLib.Interfaces.Entities
{
    public interface IUpdatable<in T>
    {
        void Update(T update);
    }
}