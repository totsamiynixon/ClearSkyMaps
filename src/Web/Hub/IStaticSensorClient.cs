using Web.Models.Hub;

namespace Web.Hub
{
    public interface IStaticSensorClient
    {
        void DispatchReading(StaticSensorReadingDispatchModel reading);
    }
}
