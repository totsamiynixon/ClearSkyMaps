using Services.DTO.Enums;
using Services.DTO.Sensor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ISensorService
    {
        Task<bool> CheckTrackingKeyAsync(string trackingKey);

        Task<string> RegisterAndGetTrackingKeyAsync(RegisterSensorDTO sensor);

        Task<List<SensorInfoDTO>> GetSensorListAsync();

        Task<int> GetSensorIdByTrackingKeyAsync(string trackingKey);

        Task<(int sensorId, PollutionLevel level)> GetSensorPollutionLevelInfoAsync(string trackingKey);
    }
}
