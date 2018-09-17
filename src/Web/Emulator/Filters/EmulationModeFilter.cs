using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Web.Emulator.Filters
{
    public class EmulationModeFilter : ActionFilterAttribute
    {
        public override Task OnActionExecutingAsync(HttpActionContext actionContext,
          CancellationToken cancellationToken)
        {
            if (actionContext.Request.RequestUri.LocalPath.Contains("api") && Emulator.IsEmulationEnabled)
            {
                if (actionContext.Request.RequestUri.LocalPath.Contains("api/sensors"))
                {
                    var response = actionContext.Request.CreateResponse(HttpStatusCode.Redirect);
                    response.Headers.Location = new Uri(actionContext.Request.RequestUri.OriginalString.Replace("api/sensors", "api/emulation"));
                    response.Content = actionContext.Request.Content;
                    actionContext.Response = response;
                }
                else if(!actionContext.Request.RequestUri.LocalPath.Contains("api/emulation"))
                {
                    var response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
                    actionContext.Response = response;
                }
            }
            return base.OnActionExecutingAsync(actionContext, cancellationToken);
        }

    }
}