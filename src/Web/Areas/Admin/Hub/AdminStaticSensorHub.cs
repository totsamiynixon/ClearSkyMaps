using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Microsoft.AspNet.SignalR;
using Web.Data.Models;
using Web.Models.Hub;

namespace Web.Areas.Admin.Hub
{
    [Authorize]
    public class AdminStaticSensorHub : Microsoft.AspNet.SignalR.Hub<IAdminStaticSensorClient>
    {
     
    }
}