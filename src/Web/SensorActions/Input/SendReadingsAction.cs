using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Enum;

namespace Web.SensorActions.Input
{
    public class SendReadingsAction : InputSensorAction<SendReadingsActionPayload>
    {
        public SendReadingsAction(InputSensorActionType type, SendReadingsActionPayload payload) : base(type, payload)
        {
        }
    }
}