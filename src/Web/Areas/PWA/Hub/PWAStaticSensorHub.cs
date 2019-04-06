using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Microsoft.AspNet.SignalR;
using Web.Data.Models;
using Web.Models.Hub;

namespace Web.Hub
{
    public class PWAStaticSensorHub : Microsoft.AspNet.SignalR.Hub<IPWAStaticSensorClient>
    {
     
    }
}