using Mobile.Models.Enums;
using Redux;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.Core.Store.Actions
{
    public class SetFilterParameterAction : IAction
    {
        public Parameters Payload { get; private set; }

        public SetFilterParameterAction(Parameters payload)
        {
            Payload = payload;
        }
    }
}
