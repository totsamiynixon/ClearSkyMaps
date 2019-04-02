using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Web.Areas.Admin.Filters;
using Web.Areas.Admin.Models.Sensors;
using Web.Data;
using Web.Data.Models;
using Web.Helpers;
using Web.SensorActions.Output;
using Z.EntityFramework.Plus;

namespace Web.Areas.Admin.Controllers
{
    public class SensorsController : BaseController
    {
        private readonly DataContext _context;
        private static readonly IMapper _mapper = new Mapper(new MapperConfiguration(x =>
        {
            x.CreateMap<Sensor, SensorListItemViewModel>()
            .ForMember(f => f.PollutionLevel, m => m.ResolveUsing(f => PollutionHelper.CalculatePollutionLevel(f.Readings)))
            .ForMember(f => f.IsConnected, m => m.ResolveUsing(f => SensorWebSocketHelper.IsConnected(f.Id)));
            x.CreateMap<Sensor, SensorDetailsViewModel>();
            x.CreateMap<Sensor, ActivateSensorViewModel>().ForMember(f => f.Details, m => m.MapFrom(f => f));
            x.CreateMap<Sensor, ChangeVisibilitySensorViewModel>().ForMember(f => f.Details, m => m.MapFrom(f => f));
            x.CreateMap<Sensor, DeleteSensorViewModel>().ForMember(f => f.Details, m => m.MapFrom(f => f));
            x.CreateMap<CreateSensorModel, Sensor>();
            x.CreateMap<Sensor, PushStateActionPayload>();
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
                       SensorWebSocketHelper.TriggerChangeState(sensor);
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
            SensorWebSocketHelper.TriggerChangeState(sensor);
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
        public async Task<ActionResult> Connect(SensorConnectDisconnectModel model)
        {
            if (!ModelState.IsValid)
            {
                ShowAlert(Enums.AlertTypes.Warning, "Ошибка при попытке подключиться к датчику!");
                return RedirectToAction("Index");
            }
            var sensor = await _context.Sensors.FirstOrDefaultAsync(f => f.Id == model.Id);
            if (sensor == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, "Датчик с таким id не найден");
            }
            if (SensorWebSocketHelper.IsConnected(sensor.Id))
            {
                ShowAlert(Enums.AlertTypes.Warning, "Сервер уже подключен к этому датчику!");
                return RedirectToAction("Index");
            }
            try
            {
                SensorWebSocketHelper.ConnectSensor(sensor);
            }
            catch
            {
                ShowAlert(Enums.AlertTypes.Error, "Что-то пошло не так!");
                return RedirectToAction("Index");
            }
            ShowAlert(Enums.AlertTypes.Success, "Подключено успешно!");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Disconnect(SensorConnectDisconnectModel model)
        {
            if (!ModelState.IsValid)
            {
                ShowAlert(Enums.AlertTypes.Warning, "Ошибка при попытке отключиться от датчика!");
                return RedirectToAction("Index");
            }
            var sensor = await _context.Sensors.FirstOrDefaultAsync(f => f.Id == model.Id);
            if (sensor == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, "Датчик с таким id не найден");
            }
            if (!SensorWebSocketHelper.IsConnected(sensor.Id))
            {
                ShowAlert(Enums.AlertTypes.Warning, "Сервер еще не подключек к этому датчику!");
                return RedirectToAction("Index");
            }
            SensorWebSocketHelper.DisconnectSensor(sensor.Id);
            ShowAlert(Enums.AlertTypes.Success, "Успешно отключен!");
            return RedirectToAction("Index");
        }


    }
}