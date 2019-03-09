using System.Configuration;
using System.Web.Http;
using Web.Emulation;

namespace Web.Controllers
{
    [RoutePrefix("api/emulation")]
    public class EmulationController : ApiController
    {
        [HttpGet]
        [Route("start")]
        public IHttpActionResult StartEmulation()
        {
            if (!bool.Parse(ConfigurationManager.AppSettings["emulation:enabled"].ToString()))
            {
                return BadRequest();
            }
            Emulator.RunEmulation();
            return Ok("Emulation was enabled!");
        }

        [HttpGet]
        [Route("stop")]
        public IHttpActionResult StopEmulation()
        {
            if (!bool.Parse(ConfigurationManager.AppSettings["emulation:enabled"].ToString()))
            {
                return BadRequest();
            }
            Emulator.StopEmulation();
            return Ok("Emulation was stopped!");
        }

    }
}
