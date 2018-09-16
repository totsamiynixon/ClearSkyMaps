using Services.DTO.Reading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.Hub
{
    public class SensorReadingDispatchModel
    {
        public int SensorId { get; set; }

        public SensorReadingDTO Reading { get; set; }
    }
}