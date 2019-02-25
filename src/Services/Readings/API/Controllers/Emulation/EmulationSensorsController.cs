using Microsoft.AspNetCore.Mvc;
using Readings.Services.DTO.Sensor;
using Readings.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using Readings.API.Emulator;

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
        public async Task<IActionResult> StartEmulationAsync()
        {
            await _emulator.RunEmulationAsync();
            return Ok("Emulation was enabled!");
        }

        [HttpGet]
        [Route("stop")]
        public async Task<IActionResult> StopEmulationAsync()
        {
            await _emulator.StopEmulationAsync();
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