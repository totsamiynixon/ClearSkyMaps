﻿//using Readings.DAL;
//using Readings.DAL.Intarfaces;
//using Readings.Services.DTO.Attributes;
//using Readings.Services.DTO.Enums;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Net;
//using System.Net.Http;
//using System.Threading.Tasks;
//using System.Web.Http;

//namespace Web.Controllers.Api
//{
//    [Route("api/analytics")]
//    public class AnalyticsController : Controller
//    {

//        private readonly IDataContext _db;
//        private readonly IDbSet<Reading> _readingsRepository;
//        private static Dictionary<LevelOfDanger, double> _ai = new Dictionary<LevelOfDanger, double>
//        {
//            {LevelOfDanger.Low, 1.5},
//            {LevelOfDanger.Medium, 1.3},
//            {LevelOfDanger.High, 1.0},
//            {LevelOfDanger.UltraHigh, 0.85}
//        };
//        public AnalyticsController(IDataContext context)
//        {
//            _db = context;
//            _readingsRepository = _db.GetRepository<Reading>();
//        }
//        [HttpGet]
//        [Route("getParameters")]
//        public IHttpActionResult GetProperties()
//        {
//            var names = typeof(Reading).GetProperties().Where(f => f.CustomAttributes.Any(z => z.AttributeType == typeof(PDK))).Select(f => f.Name);
//            return Ok(names);
//        }

//        [HttpGet]
//        [Route("getAll")]
//        public async Task<IHttpActionResult> GetValuesByPeriodAsync(int skip, int count, DateTime startPeriod, DateTime endPeriod, int sensorId, int? everyNth = null)
//        {
//            if (startPeriod > endPeriod)
//            {
//                return BadRequest("Invalid dates");
//            }
//            var items = await _readingsRepository.Where(f => f.Created >= startPeriod && f.Created < endPeriod && f.SensorId == sensorId && !everyNth.HasValue || f.Id % everyNth.Value == 0).OrderBy(s => s.Id).Skip(skip).Take(count).ToListAsync();
//            return Ok(items);
//        }

//        [HttpGet]
//        [Route("getIZA")]
//        public async Task<IHttpActionResult> GetIZA(int sensorId)
//        {
//            var properties = typeof(Reading).GetProperties().Where(property => property.CustomAttributes.Any(z => z.AttributeType == typeof(PDK)));
//            if (properties == null)
//            {
//                return BadRequest("No properties with PDK");
//            }
//            List<(PDK pdk, double avg)> dictionary = new List<(PDK pdk, double avg)>();
//            foreach (var property in properties)
//            {
//                var pdk = property.GetCustomAttributes(typeof(PDK), false).FirstOrDefault() as PDK;
//                if (pdk != null)
//                {
//                    var avg = await _readingsRepository.Where(f => f.Created.Year == DateTime.UtcNow.Year && f.SensorId == sensorId).Select(property.Name).Cast<float>().AverageAsync();
//                    dictionary.Add((pdk, avg));
//                }
//            }

//            var result = dictionary.Select(f => Math.Pow((f.avg / f.pdk.PDKValue), _ai[f.pdk.LevelOfDanger])).Sum();
//            return Ok(result);
//        }

//        [HttpGet]
//        [Route("getSI")]
//        public async Task<IHttpActionResult> GetSI(string parameterName, DateTime startPeriod, DateTime endPeriod, int sensorId, int? everyNth = null)
//        {
//            if (startPeriod > endPeriod)
//            {
//                return BadRequest("Invalid dates");
//            }
//            var property = typeof(Reading).GetProperty(parameterName);
//            if (property == null && property.CustomAttributes.Any(z => z.AttributeType == typeof(PDK)))
//            {
//                return BadRequest("No such parameter exists");
//            }
//            var pdk = property.GetCustomAttributes(typeof(PDK), false).FirstOrDefault() as PDK;
//            var maxValue = await _readingsRepository.Where(f => f.Created >= startPeriod && f.Created < endPeriod && f.SensorId == sensorId && !everyNth.HasValue || f.Id % everyNth.Value == 0).Select(parameterName).Cast<float>().MaxAsync();
//            var result = maxValue / pdk.PDKValue;
//            return Ok(result);
//        }

//        [HttpGet]
//        [Route("getNP")]
//        public async Task<IHttpActionResult> GetNP(string parameterName, DateTime startPeriod, DateTime endPeriod, int sensorId, int? everyNth = null)
//        {
//            if (startPeriod > endPeriod)
//            {
//                return BadRequest("Invalid dates");
//            }
//            var property = typeof(Reading).GetProperty(parameterName);
//            if (property == null && property.CustomAttributes.Any(z => z.AttributeType == typeof(PDK)))
//            {
//                return BadRequest("No such parameter exists");
//            }
//            var pdk = property.GetCustomAttributes(typeof(PDK), false).FirstOrDefault() as PDK;
//            var countOfOverlimits = await _readingsRepository.Where(f => f.Created >= startPeriod && f.Created < endPeriod && f.SensorId == sensorId && !everyNth.HasValue || f.Id % everyNth.Value == 0).Select(parameterName).Cast<double>().Where(f => f > pdk.PDKValue).CountAsync();
//            var totalCount = await _readingsRepository.Where(f => f.Created >= startPeriod && f.Created < endPeriod && f.SensorId == sensorId).CountAsync();
//            var result = (double)(countOfOverlimits * 100 / totalCount);
//            return Ok(result);
//        }

//        [HttpGet]
//        [Route("getPZsr")]
//        public async Task<IHttpActionResult> GetPZsr(string parameterName, DateTime startPeriod, DateTime endPeriod, int sensorId, int? everyNth = null)
//        {
//            if (startPeriod > endPeriod)
//            {
//                return BadRequest("Invalid dates");
//            }
//            var property = typeof(Reading).GetProperty(parameterName);
//            if (property == null && property.CustomAttributes.Any(z => z.AttributeType == typeof(PDK)))
//            {
//                return BadRequest("No such parameter exists");
//            }
//            var pdk = property.GetCustomAttributes(typeof(PDK), false).FirstOrDefault() as PDK;
//            var values = await _readingsRepository.Where(f => f.Created >= startPeriod && f.Created < endPeriod && f.SensorId == sensorId && !everyNth.HasValue || f.Id % everyNth.Value == 0).Select(parameterName).Cast<double>().ToListAsync();
//            var result = values.Sum(f => f / pdk.PDKValue) / values.Count;
//            return Ok(result);
//        }
//    }
//}