using DAL;
using Services.DTO.Sensor;
using Services.Interfaces;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Web.Emulator;
using Web.Models;

namespace ArduinoServer.Controllers.Api
{
    [RoutePrefix("api/emulation")]
    public class EmulationSensorsController : ApiController
    {

        [HttpGet]
        [Route("start")]
        public IHttpActionResult StartEmulation()
        {
            Emulator.RunEmulation();
            return Ok("Emulation was enabled!");
        }

        [HttpGet]
        [Route("stop")]
        public IHttpActionResult StopEmulation()
        {
            Emulator.StopEmulation();
            return Ok("Emulation was stopped!");
        }

        [HttpGet]
        [Route("limit")]
        public IHttpActionResult EmulatorLimit(double limit)
        {
            Emulator.SetMemoryLimit(limit);
            return Ok("Limit was updated to: " + limit);
        }
        [HttpGet]
        [Route("status")]
        public IHttpActionResult GetStatus()
        {
            return Ok($"Emulation is {(Emulator.IsEmulationEnabled ? "enabled" : "stopped")}!");
        }


    }
}