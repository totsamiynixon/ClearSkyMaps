using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.Core.Services.Interfaces
{
    public interface INonUITasksManager
    {
        void BeginInvokeInNonUIThread(Action action);
    }
}
