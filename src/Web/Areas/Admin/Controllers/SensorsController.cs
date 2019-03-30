using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web.Areas.Admin.Filters;
using Web.Areas.Admin.Models.Sensors;
using Web.Data;
using Web.Data.Models;
using Web.Enum;
using Web.Helpers;
using Z.EntityFramework.Plus;

namespace Web.Areas.Admin.Controllers
{
    public class SensorsController : BaseController
    {
        private readonly DataContext _context;
        private static readonly IMapper _mapper = new Mapper(new MapperConfiguration(x =>
        {
            x.CreateMap<Sensor, SensorListItemViewModel>().ForMember(f => f.PollutionLevel, m => m.ResolveUsing(f => PollutionHelper.CalculatePollutionLevel(f.Readings)));
            x.CreateMap<Sensor, SensorDetailsViewModel>();
            x.CreateMap<Sensor, ActivateSensorViewModel>().ForMember(f => f.Details, m => m.MapFrom(f => f));
            x.CreateMap<Sensor, ChangeVisibilitySensorViewModel>().ForMember(f => f.Details, m => m.MapFrom(f => f));
            x.CreateMap<Sensor, DeleteSensorViewModel>().ForMember(f => f.Details, m => m.MapFrom(f => f));
            x.CreateMap<CreateSensorModel, Sensor>();
            x.CreateMap<Sensor, PairModel>();
        }));

        public SensorsController()
        {
            _context = new DataContext();
        }
        // GET: Admin/Sensors
        public async Task<ActionResult> Index()
        {
            var sensors = await _context
                          .Sensors
                          .Where(s => !s.IsDeleted)
                          .IncludeFilter(f => f.Readings
                                               .OrderByDescending(s => s.Created)
                                               .Take(10))
                          .ToListAsync();
            var model = _mapper.Map<List<Sensor>, List<SensorListItemViewModel>>(sensors);
            return View(model);
        }


        [HttpGet]
        [RestoreModelStateFromTempData]
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [SetTempDataModelState]
        public async Task<ActionResult> Create(CreateSensorModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Create");
            }
            var sensor = _mapper.Map<CreateSensorModel, Sensor>(model);
            try
            {
                sensor.TrackingKey = Guid.NewGuid().ToString();
                _context.Sensors.Add(sensor);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                SqlException innerException = null;
                Exception tmp = e;
                while (innerException == null && tmp != null)
                {
                    if (tmp != null)
                    {
                        innerException = tmp.InnerException as SqlException;
                        tmp = tmp.InnerException;
                    }

                }
                if (innerException != null && innerException.Number == 2601)
                {
                    ModelState.AddModelError("Insert", innerException.Message);
                }
                else
                {
                    throw;
                }
            }
            ShowAlert(Enums.AlertTypes.Success, "Датчик был успешно зарегистрирован!");
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Supervisor")]
        public async Task<ActionResult> Delete(int? sensorId)
        {
            if (!sensorId.HasValue)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, "Необходим id датчика!");
            }
            var sensor = await _context.Sensors.FirstOrDefaultAsync(f => f.Id == sensorId);
            if (sensor == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, "Датик с таким id не найден");
            }
            var mappedSensor = _mapper.Map<Sensor, DeleteSensorViewModel>(sensor);
            return View(mappedSensor);
        }


        [HttpGet]
        [Authorize(Roles = "Supervisor")]
        public async Task<ActionResult> Delete(DeleteSensorModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Delete", new { sensorId = model.Id });
            }
            var sensor = await _context.Sensors.FirstOrDefaultAsync(f => f.Id == model.Id);
            if (sensor == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, "Датчик с таким id не найден");
            }
            sensor.IsDeleted = true;
            _context.Entry(sensor).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            ShowAlert(Enums.AlertTypes.Success, "Удаление датчика прошло успешно!");
            return RedirectToAction("Index");
        }


        [HttpGet]
        [RestoreModelStateFromTempData]
        public async Task<ActionResult> ChangeActivation(int? sensorId)
        {
            if (!sensorId.HasValue)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, "Необходим id датчика!");
            }
            var sensor = await _context.Sensors.FirstOrDefaultAsync(f => f.Id == sensorId.Value);
            if (sensor == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, "Датчик с таким id не найден");
            }
            var mappedSensor = _mapper.Map<Sensor, ActivateSensorViewModel>(sensor);
            return View(mappedSensor);
        }


        [HttpPost]
        [SetTempDataModelState]
        public async Task<ActionResult> ChangeActivation(ActivateSensorModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("ChangeActivation", new { sensorId = model.Id }); ;
            }
            var sensor = await _context.Sensors.FirstOrDefaultAsync(f => f.Id == model.Id);
            if (sensor == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, "Датчик с таким id не найден");
            }
            sensor.IsActive = model.IsActive.Value;
            _context.Entry(sensor).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            ShowAlert(Enums.AlertTypes.Success, $"{(sensor.IsActive ? "Активация" : "Деактивация")} датчика прошло успешно!");
            return RedirectToAction("Index");
        }

        [HttpGet]
        [RestoreModelStateFromTempData]
        public async Task<ActionResult> ChangeVisibility(int? sensorId)
        {
            if (!sensorId.HasValue)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, "Необходим id датчика!");
            }
            var sensor = await _context.Sensors.FirstOrDefaultAsync(f => f.Id == sensorId.Value);
            if (sensor == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, "Датчик с таким id не найден");
            }
            var mappedSensor = _mapper.Map<Sensor, ChangeVisibilitySensorViewModel>(sensor);
            return View(mappedSensor);
        }


        [HttpPost]
        [SetTempDataModelState]
        public async Task<ActionResult> ChangeVisibility(ChangeVisibilitySensorModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("ChangeVisibility", new { sensorId = model.Id }); ;
            }
            var sensor = await _context.Sensors.FirstOrDefaultAsync(f => f.Id == model.Id);
            if (sensor == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, "Датчик с таким id не найден");
            }
            sensor.IsActive = model.IsVisible.Value;
            _context.Entry(sensor).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            ShowAlert(Enums.AlertTypes.Success, $"{(sensor.IsActive ? "Отображение" : "Скрытие")} датчика прошло успешно!");
            return RedirectToAction("Index");
        }


        [HttpPost]
        [Authorize(Roles = "Supervisor")]
        public async Task<ActionResult> Pair(PairUnpairSensorModel model)
        {
            if (!ModelState.IsValid)
            {
                ShowAlert(Enums.AlertTypes.Warning, "Ошибка при попытке спарить датчик!");
                return RedirectToAction("Index");
            }
            var sensor = await _context.Sensors.FirstOrDefaultAsync(f => f.Id == model.SensorId);
            if (sensor == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, "Датчик с таким id не найден");
            }
            if (sensor.IsPaired)
            {
                ShowAlert(Enums.AlertTypes.Warning, "Датчик уже спарен!");
                return RedirectToAction("Index");
            }
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsync($"http://{sensor.IPAddress}/pair", new StringContent(JsonConvert.SerializeObject(_mapper.Map<Sensor, PairModel>(sensor)), Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    sensor.IsPaired = true;
                    _context.Entry(sensor).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    ShowAlert(Enums.AlertTypes.Success, "Датчик успешно спарен!");
                    return RedirectToAction("Index");
                }
                else
                {
                    ShowAlert(Enums.AlertTypes.Error, "Провалилась попытка спарить датчик!!");
                    return RedirectToAction("Index");
                }
            }
        }

        [HttpPost]
        [Authorize(Roles = "Supervisor")]
        public async Task<ActionResult> Unpair(int? sensorId)
        {
            if (!sensorId.HasValue)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, "Необходим id датчика!");
            }
            var sensor = await _context.Sensors.FirstOrDefaultAsync(f => f.Id == sensorId.Value);
            if (sensor == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, "Датчик с таким id не найден");
            }
            if (!sensor.IsPaired)
            {
                ShowAlert(Enums.AlertTypes.Warning, "Датчик еще не спарен!");
                return RedirectToAction("Index");
            }
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsync($"http://{sensor.IPAddress}/unpair", new StringContent("", Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    sensor.IsPaired = false;
                    _context.Entry(sensor).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    ShowAlert(Enums.AlertTypes.Success, "Датчик успешно отстыкован!");
                    return RedirectToAction("Index");
                }
                else
                {
                    ShowAlert(Enums.AlertTypes.Error, "Провалилась попытка отстыковать датчик!!");
                    return RedirectToAction("Index");
                }
            }
        }


    }
}