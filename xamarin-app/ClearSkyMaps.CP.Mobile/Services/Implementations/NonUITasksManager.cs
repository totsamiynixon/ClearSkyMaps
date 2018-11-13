using ClearSkyMaps.CP.Mobile.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClearSkyMaps.CP.Mobile.Services.Implementations
{

    public class NonUITasksManager : INonUITasksManager
    {
        private readonly ConcurrentExclusiveSchedulerPair Scheduler;
        public NonUITasksManager()
        {
            Scheduler = new ConcurrentExclusiveSchedulerPair();
        }

        public void BeginInvokeInNonUIThread(Action action)
        {
            Task.Factory.StartNew(() =>
            {
                action?.Invoke();
            }, CancellationToken.None, TaskCreationOptions.DenyChildAttach, Scheduler.ExclusiveScheduler);
        }
    }
}
