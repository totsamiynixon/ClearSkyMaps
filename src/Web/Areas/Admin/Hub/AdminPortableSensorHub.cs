using Microsoft.AspNet.SignalR;
namespace Web.Areas.Admin.Hub
{
    [Authorize]
    public class AdminPortableSensorHub : Microsoft.AspNet.SignalR.Hub<IAdminPortableSensorClient>
    {
        public static string PortableSensorGroup(int sensorId) => $"PortableSensorGroup_{sensorId}";

        public void ListenForSensor(int sensorId)
        {
            Groups.Add(Context.ConnectionId, PortableSensorGroup(sensorId));
        }
    }
}