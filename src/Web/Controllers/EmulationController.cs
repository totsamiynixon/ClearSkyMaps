using System.Configuration;
using System.Threading.Tasks;
using System.Web.Http;
using Web.Emulation;

namespace Web.Controllers
{
    [RoutePrefix("api/emulation")]
    public class EmulationController : ApiController
    {
        [HttpGet]
        [Route("start")]
        public async Task<IHttpActionResult> StartEmulationAsync()
        {
            if (!bool.Parse(ConfigurationManager.AppSettings["emulation:enabled"].ToString()))
            {
                return BadRequest();
            }
            await Emulator.RunEmulationAsync();
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
