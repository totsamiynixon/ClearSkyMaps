using System;
using System.Collections.Generic;
using System.Text;

namespace Readings.Services.DTO.Models.Reading
{
    public class SaveReadingResultDTO
    {
        public DateTime Created { get; set; }

        public int SensorId { get; set; }

        public int Id { get; set; }
    }
}
