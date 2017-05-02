namespace DevilHome.Controller.Database
{
    public interface IDatabaseBase
    {
        void InitDatabase();
        void InsertSensorValue(string sensorType, string roomName, string sensorName, double sensorValue);
    }
}