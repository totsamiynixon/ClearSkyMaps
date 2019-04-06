using Web.Models.Hub;

namespace Web.Areas.Admin.Hub
{
    public interface IAdminStaticSensorClient
    {
        void DispatchReading(StaticSensorReadingDispatchModel reading);
    }
}
