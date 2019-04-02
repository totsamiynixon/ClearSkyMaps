using Microsoft.AspNet.SignalR;
namespace Web.Areas.Admin.Hub
{
    [Authorize]
    public class PortableSensorHub : Microsoft.AspNet.SignalR.Hub<IPortableSensorClient>
    {
    }
}