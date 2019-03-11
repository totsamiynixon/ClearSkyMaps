using System.Threading.Tasks;
using System.Web.Http;
using Web.Emulation;
using Web.Helpers;

namespace Web.Controllers
{
    [RoutePrefix("api/emulation")]
    public class EmulationController : ApiController
    {
        [HttpGet]
        [Route("start")]
        public async Task<IHttpActionResult> StartEmulationAsync()
        {
            if (SettingsHelper.EmulationEnabled)
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
            if (SettingsHelper.EmulationEnabled)
            {
                return BadRequest();
            }
            Emulator.StopEmulation();
            return Ok("Emulation was stopped!");
        }

    }
}
