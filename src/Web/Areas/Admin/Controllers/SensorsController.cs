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
        private static readonly IMapper _mapper = new Mapper(new MapperConfiguration(x =>
        {
            x.CreateMap<Sensor, SensorListItemViewModel>()
            .ForMember(f => f.IsConnected, m => m.MapFrom(f => SensorWebSocketHelper.IsConnected(f.Id)));
            x.CreateMap<StaticSensor, StaticSensorListItemViewModel>()
            //.ForMember(f => f.PollutionLevel, m => m.MapFrom(f => PollutionHelper.GetPollutionLevel(f.Id)))
            .IncludeBase<Sensor, SensorListItemViewModel>();
            x.CreateMap<Sensor, SensorDetailsViewModel>();
            x.CreateMap<StaticSensor, StaticSensorDetailsViewModel>()
            .IncludeBase<Sensor, SensorDetailsViewModel>();
            x.CreateMap<Sensor, ActivateSensorViewModel>().ForMember(f => f.Details, m => m.MapFrom(f => f));
            x.CreateMap<Sensor, ChangeVisibilityStaticSensorViewModel>().ForMember(f => f.Details, m => m.MapFrom(f => f));
            x.CreateMap<Sensor, DeleteSensorViewModel>().ForMember(f => f.Details, m => m.MapFrom(f => f));
            x.CreateMap<CreateStaticSensorModel, Sensor>();
            x.CreateMap<Sensor, PushStateActionPayload>();
        }));

        // GET: Admin/Sensors
        public async Task<ActionResult> Index()
        {

            var sensors = await DatabaseHelper.GetSensorsAsync();
            var model = new SensorsIndexViewModel
            {
                PortableSensors = _mapper.Map<List<PortableSensor>, List<SensorListItemViewModel>>(sensors.OfType<PortableSensor>().ToList()),
                StaticSensors = _mapper.Map<List<StaticSensor>, List<StaticSensorListItemViewModel>>(sensors.OfType<StaticSensor>().ToList()),
            };
            return View(model);
        }


        [HttpGet]
        [RestoreModelStateFromTempData]
        public ActionResult CreateStaticSensor()
        {
            return View();
        }


        [HttpPost]
        [SetTempDataModelState]
        public async Task<ActionResult> CreateStaticSensor(CreateStaticSensorModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Create");
            }
            try
            {
                await DatabaseHelper.AddStaticSensorAsync(model.IPAddress, model.Latitude, model.Longitude);
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
            ShowAlert(Enums.AlertTypes.Success, "Статический датчик был успешно зарегистрирован!");
            return RedirectToAction("Index");
        }

        [HttpGet]
        [RestoreModelStateFromTempData]
        public ActionResult CreatPortableSensor()
        {
            return View();
        }


        [HttpPost]
        [SetTempDataModelState]
        public async Task<ActionResult> CreatePortableSensor(CreatePortableSensorModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Create");
            }
            try
            {
                await DatabaseHelper.AddPortableSensor(model.IPAddress);
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
            ShowAlert(Enums.AlertTypes.Success, "Портативный датчик был успешно зарегистрирован!");
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
            var sensor = await DatabaseHelper.GetSensorByIdAsync(sensorId.Value);
            if (sensor == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, "Датик с таким id не найден");
            }
            if (sensor.IsActive)
            {
                ShowAlert(Enums.AlertTypes.Success, "Нельзя удалить активный датчик!");
                return RedirectToAction("Index");
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
            await DatabaseHelper.RemoveSensorAsync(model.Id.Value);
            SensorWebSocketHelper.TriggerChangeState(await DatabaseHelper.GetSensorByIdAsync(model.Id.Value));
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
            var sensor = await DatabaseHelper.GetSensorByIdAsync(sensorId.Value);
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
            var sensor = await DatabaseHelper.ChangeSensorActivationAsync(model.Id.Value, model.IsActive.Value);
            if (sensor is StaticSensor)
            {
                await SensorCacheHelper.UpdateStaticSensorCache(sensor as StaticSensor);
            }
            SensorWebSocketHelper.TriggerChangeState(sensor);
            ShowAlert(Enums.AlertTypes.Success, $"{(sensor.IsActive ? "Активация" : "Деактивация")} датчика прошло успешно!");
            return RedirectToAction("Index");
        }

        [HttpGet]
        [RestoreModelStateFromTempData]
        public async Task<ActionResult> ChangeVisibilityStaticSensor(int? sensorId)
        {
            if (!sensorId.HasValue)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, "Необходим id датчика!");
            }
            var sensor = await DatabaseHelper.GetSensorByIdAsync(sensorId.Value);
            var mappedSensor = _mapper.Map<Sensor, ChangeVisibilityStaticSensorViewModel>(sensor);
            return View(mappedSensor);
        }


        [HttpPost]
        [SetTempDataModelState]
        public async Task<ActionResult> ChangeVisibilityStaticSensor(ChangeVisibilityStaticSensorModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("ChangeVisibility", new { sensorId = model.Id }); ;
            }
            var sensor = await DatabaseHelper.GetSensorByIdAsync(model.Id.Value);
            if (sensor == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, "Датчик с таким id не найден");
            }
            sensor = await DatabaseHelper.UpdateStaticSensorVisibility(model.Id.Value, model.IsVisible.Value);
            await SensorCacheHelper.UpdateStaticSensorCache(sensor as StaticSensor);
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
            var sensor = await DatabaseHelper.GetSensorByIdAsync(model.Id);
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
            var sensor = await DatabaseHelper.GetSensorByIdAsync(model.Id);
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

        public async Task<ActionResult> PortableSensorDetails(int? sensorId)
        {
            if (!sensorId.HasValue)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, "Необходим id датчика!");
            }
            var sensor = await DatabaseHelper.GetSensorByIdAsync(sensorId.Value);
            if (sensor == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, "Датик с таким id не найден");
            }
            return View(sensorId.Value);
        }


    }
}