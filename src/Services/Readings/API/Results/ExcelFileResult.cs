using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace Readings.API.Results
{
    public class ExcelFileResult<T> : ActionResult
    {
        public ExcelFileResult(IEnumerable<T> data, string fileName)
        {
            _data = data;
            _fileName = fileName;
        }

        //Name the property Data and make the getter public
        private readonly IEnumerable<T> _data;

        private readonly string _fileName;

        public override void ExecuteResult(ActionContext context)
        {
            base.ExecuteResult(context);
        }

    }
}