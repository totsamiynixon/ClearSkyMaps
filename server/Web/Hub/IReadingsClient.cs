using Web.Models.Hub;

namespace Web.Hub
{
    public interface IReadingsClient
    {
        void DispatchReading(SensorReadingDispatchModel reading);
    }
}
