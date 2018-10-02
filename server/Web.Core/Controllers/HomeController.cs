﻿
using Microsoft.AspNetCore.Mvc;
using Services.DTO.Reading;
using Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Web.Core.Results;

namespace Web.Core.Controllers
{
    public class HomeController : Controller
    {
        private readonly IReadingService _readingService;
        public HomeController(IReadingService readingService)
        {
            _readingService = readingService;
        }

        public ActionResult Readings()
        {
            return View();
        }

        public ActionResult Chart(int sensorId)
        {
            return View(sensorId);
        }

        public ActionResult Analytics(int sensorId)
        {
            return View(sensorId);
        }

        public async Task<ActionResult> ExportValuesByPeriod(DateTime startPeriod, DateTime endPeriod, int sensorId, int? everyNth = null)
        {
            if (startPeriod > endPeriod)
            {
                throw new ArgumentException("Invalid dates");
            }
            var items = await _readingService.GetReadingsForExportAsync(startPeriod, endPeriod, sensorId, everyNth);
            return new ExcelFileResult<SensorReadingDTO>(items, "Readings");
        }
    }
}