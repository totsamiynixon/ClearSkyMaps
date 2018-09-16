using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Results
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

        public override void ExecuteResult(ControllerContext context)
        {
            var json = JsonConvert.SerializeObject(_data);
            DataTable dt = (DataTable)JsonConvert.DeserializeObject(json, (typeof(DataTable)));
            var gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();
            context.HttpContext.Response.ClearContent();
            context.HttpContext.Response.Buffer = true;
            context.HttpContext.Response.AddHeader("content-disposition", $"attachment; filename={_fileName}.xls");
            context.HttpContext.Response.ContentType = "application/ms-excel";
            context.HttpContext.Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlTextWriter);
            context.HttpContext.Response.Output.Write(objStringWriter.ToString());
            context.HttpContext.Response.Flush();
            context.HttpContext.Response.End();
        }
    }
}