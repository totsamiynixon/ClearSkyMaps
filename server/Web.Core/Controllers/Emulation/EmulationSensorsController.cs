using Microsoft.AspNetCore.Mvc;
using Services.DTO.Sensor;
using Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using Web.Core.Emulator;

namespace ArduinoServer.Controllers.Api
{
    [Route("api/emulation")]
    public class EmulationSensorsController : Controller
    {
        private readonly Emulator _emulator;
        public EmulationSensorsController(Emulator emulator)
        {
            _emulator = emulator;
        }

        [HttpGet]
        [Route("start")]
        public IActionResult StartEmulation()
        {
            _emulator.RunEmulation();
            return Ok("Emulation was enabled!");
        }

        [HttpGet]
        [Route("stop")]
        public IActionResult StopEmulation()
        {
            _emulator.StopEmulation();
            return Ok("Emulation was stopped!");
        }

        [HttpGet]
        [Route("limit")]
        public IActionResult EmulatorLimit(double limit)
        {
            _emulator.SetMemoryLimit(limit);
            return Ok("Limit was updated to: " + limit);
        }
        [HttpGet]
        [Route("status")]
        public IActionResult GetStatus()
        {
            return Ok($"Emulation is {(_emulator.IsEmulationEnabled ? "enabled" : "stopped")}!");
        }


    }
}