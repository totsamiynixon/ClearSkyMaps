using Web.Models.Hub;

namespace Web.Hub
{
    public interface IPWAStaticSensorClient
    {
        void DispatchReading(StaticSensorReadingDispatchModel reading);
    }
}
