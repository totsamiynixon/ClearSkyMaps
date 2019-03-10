using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Data.Models
{
    public class Subscriber
    {
        public string Id { get; set; }

        public List<Sensor> Sensors { get; set; }

        public List<Notification> UnreadNotifications { get; set; }

    }
}