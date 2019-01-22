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
        ///// <summary>
        ///// Checks is sensor with such tracking key is regirested in system 
        ///// </summary>
        ///// <param name="trackingKey"></param>
        ///// <returns></returns>
        //Task<bool> CheckTrackingKeyAsync(string trackingKey);

        /// <summary>
        /// Every sensor should be registered in system to be able to send data. Use this method to do it.
        /// </summary>
        /// <param name="sensor">Object with data about sensor</param>
        /// <returns> Tracking key for registered sensor </returns>
        Task<string> RegisterAndGetTrackingKeyAsync(RegisterSensorDTO sensor);

        /// <summary>
        /// Returns list of all sensors.
        /// </summary>
        /// <returns> List of sensors with readings </returns>
        Task<List<SensorInfoDTO>> GetSensorListAsync();

        /// <param name="trackingKey">Sensor's tracking key</param>
        /// <returns>Id of sensor with such tracking key if exists and 0 if doesn't</returns>
        Task<int> GetSensorIdByTrackingKeyAsync(string trackingKey);

        /// <summary>
        /// Info about current sensor pollution level
        /// </summary>
        /// <param name="trackingKey">Sensor tracking key</param>
        /// <returns>Current pollution level for sensor with provided tracking key</returns>
        Task<PollutionLevel> GetSensorPollutionLevelInfoAsync(string trackingKey);
    }
}
