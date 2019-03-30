using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Enum;

namespace Web.SensorActions.Output
{
    public class SendStateAction : OutputSensorAction<SendStateActionPayload>
    {
        public SendStateAction(OuputSensorActionType type, SendStateActionPayload payload) : base(type, payload)
        {
        }
    }
}