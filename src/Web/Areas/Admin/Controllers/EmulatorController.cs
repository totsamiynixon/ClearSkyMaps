using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web.Data;
using Web.Helpers;
using Web.Areas.Admin.Emulation;
using AutoMapper;
using Web.Areas.Admin.Models.Emulator;
using System.Collections.Generic;
using System.Linq;

namespace Web.Areas.Admin.Controllers
{
    public class EmulatorController : BaseController
    {
        private readonly DataContext _context;
        private static readonly IMapper _mapper = new Mapper(new MapperConfiguration(x =>
        {
            x.CreateMap<SensorEmulator, SensorEmulatorListItemViewModel>()
            .ForMember(f => f.IsActive, m => m.ResolveUsing(s => s.State?.IsActive))
            .ForMember(f => f.Latitude, m => m.ResolveUsing(s => s.State?.Latitude))
            .ForMember(f => f.Longitude, m => m.ResolveUsing(s => s.State?.Longitude))
            .ForMember(f => f.Guid, m => m.ResolveUsing(s => s.GetGuid()))
            .ForMember(f => f.IsOn, m => m.ResolveUsing(s => s.IsPowerOn));

        }));

        public EmulatorController()
        {
            _context = new DataContext();
        }

        [HttpGet]
        public ActionResult Index()
        {
            List<SensorEmulator> emulators = new List<SensorEmulator>();
            if (Emulator.IsEmulationEnabled)
            {
                emulators = Emulator.Devices;
            }
            return View(_mapper.Map<List<SensorEmulator>, List<SensorEmulatorListItemViewModel>>(emulators));
        }


        [HttpPost]
        public async Task<ActionResult> StartEmulation()
        {
            if (!SettingsHelper.EmulationEnabled)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, "Емуляция недоступна в данной среде");
            }
            await Emulator.RunEmulationAsync();
            ShowAlert(Enums.AlertTypes.Success, "Эмуляция успешно запущена!");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> StopEmulation()
        {
            if (!SettingsHelper.EmulationEnabled)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, "Емуляция недоступна в данной среде");
            }
            await Emulator.RunEmulationAsync();
            ShowAlert(Enums.AlertTypes.Success, "Эмуляция успешно остановлена!");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult PowerOn(string guid)
        {
            if (!SettingsHelper.EmulationEnabled)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, "Емуляция недоступна в данной среде");
            }
            var emulator = Emulator.Devices.Where(f => f.GetGuid() == guid).FirstOrDefault();
            if (emulator == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, "Эмулятор не найдет");
            }
            if (emulator.IsPowerOn)
            {
                ShowAlert(Enums.AlertTypes.Info, "Эмулятор уже запущен!");
            }
            else
            {
                emulator.PowerOn();
                ShowAlert(Enums.AlertTypes.Success, "Эмулятор успешно запущен!");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult PowerOff(string guid)
        {
            if (!SettingsHelper.EmulationEnabled)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, "Емуляция недоступна в данной среде");
            }
            var emulator = Emulator.Devices.Where(f => f.GetGuid() == guid).FirstOrDefault();
            if (emulator == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, "Эмулятор не найдет");
            }
            if (!emulator.IsPowerOn)
            {
                ShowAlert(Enums.AlertTypes.Info, "Эмулятор уже остановлен!");
            }
            else
            {
                emulator.PowerOff();
                ShowAlert(Enums.AlertTypes.Success, "Эмулятор успешно остановлен!");
            }
            return RedirectToAction("Index");
        }

    }
}