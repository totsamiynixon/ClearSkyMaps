using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlString ToJson<T>(this HtmlHelper helper, T model)
        {
            return helper.Raw(JsonConvert.SerializeObject(model, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }));
        }
    }
}