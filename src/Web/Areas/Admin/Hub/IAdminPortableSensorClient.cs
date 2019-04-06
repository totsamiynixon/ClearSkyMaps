using Web.Areas.Admin.Models.Hub;
using Web.Models.Hub;

namespace Web.Areas.Admin.Hub
{
    public interface IAdminPortableSensorClient
    {
        void DispatchReading(PortableSensorReadingsDispatchModel reading);
        void DispatchCoordinates(PortableSensorCoordinatesDispatchModel coordinates);
    }
}
