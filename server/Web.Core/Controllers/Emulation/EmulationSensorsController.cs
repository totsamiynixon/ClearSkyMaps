using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<EmulationSensorsController> _logger;
        public EmulationSensorsController(Emulator emulator, ILogger<EmulationSensorsController> logger)
        {
            _emulator = emulator;
            _logger = logger;
        }

        [HttpGet]
        [Route("start")]
        public async Task<IActionResult> StartEmulationAsync()
        {
            await _emulator.RunEmulationAsync();
            _logger.LogInformation("Emulation was enabled!");
            return Ok("Emulation was enabled!");
        }

        [HttpGet]
        [Route("stop")]
        public async Task<IActionResult> StopEmulationAsync()
        {
            await _emulator.StopEmulationAsync();
            _logger.LogInformation("Emulation was stopped!");
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
            _logger.LogInformation($"Emulation is {(_emulator.IsEmulationEnabled ? "enabled" : "stopped")}!");
            return Ok($"Emulation is {(_emulator.IsEmulationEnabled ? "enabled" : "stopped")}!");
        }


    }
}