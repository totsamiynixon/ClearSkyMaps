using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Enum;

namespace Web.Data.Models
{
    public abstract class Sensor
    {
        public int Id { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsActive { get; set; }

        public string IPAddress { get; set; }
    }
}