using Readings.Services.DTO.Enums;
using Readings.Services.DTO.Models.Reading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readings.Services.DTO.Sensor
{
    public class SensorInfoDTO
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public int Id { get; set; }

        public PollutionLevel LatestPollutionLevel { get; set; }

        public List<SensorReadingDTO> Readings { get; set; }
    }
}
