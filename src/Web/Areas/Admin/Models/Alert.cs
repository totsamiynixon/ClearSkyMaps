using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Areas.Admin.Enums;

namespace Web.Areas.Admin.Models
{
    public class Alert
    {
        public string Message { get; set; }

        public AlertTypes Type { get; set; }
    }
}