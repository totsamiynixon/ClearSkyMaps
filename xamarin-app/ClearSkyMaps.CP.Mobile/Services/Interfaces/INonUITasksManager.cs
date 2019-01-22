using System;
using System.Collections.Generic;
using System.Text;

namespace ClearSkyMaps.CP.Mobile.Services.Interfaces
{
    public interface INonUITasksManager
    {
        void BeginInvokeInNonUIThread(Action action);
    }
}
