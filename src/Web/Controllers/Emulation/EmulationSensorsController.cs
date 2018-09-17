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
using Z.EntityFramework.Plus;

namespace ArduinoServer.Controllers.Api
{
    [RoutePrefix("api/emulation")]
    public class EmulationSensorsController : ApiController
    {
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> Get()
        {
            return Ok(Emulator.GetSensorsData());
        }

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



    }
}