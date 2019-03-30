using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Enum;

namespace Web.SensorActions.Output
{
    public class SendStateActionPayload
    {
        public SensorType Type { get; set; }

        public bool IsActive { get; set; }

        public string TrackingKey { get; set; }

        public string WebServerIP { get; set; }

        public string WebSocketPath
        {
            get
            {

                return $"ws://{WebServerIP}/sensors/connect";
            }
        }
    }
}