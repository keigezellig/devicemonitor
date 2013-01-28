using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPCServiceTool.Common.Services.Logger;

namespace NPCServiceTool.Common.Services.DeviceMonitor
{
    public static class Extensions
    {
        /// <summary>
        /// Logs exceptions thrown in a task
        /// </summary>
        /// <param name="task"></param>
        public static void LogExceptions(this Task task, ILogger logger, EventHandler<ErrorEventErgs> errorEvent)
        {
            task.ContinueWith(t =>
            {
                var aggException = t.Exception.Flatten();
                logger.Write(Category.ERROR, task.ToString(), "", "Exceptions thrown in task");
                foreach (var exception in aggException.InnerExceptions)
                {
                    logger.Write(Category.ERROR, task.ToString(), "", exception);
                    if (errorEvent != null)
                    {
                        var handler = errorEvent;
                        errorEvent(task, new ErrorEventErgs() { Exception = exception });
                    }

                }

               
            },
            TaskContinuationOptions.OnlyOnFaulted);
        }
    }

    public class ErrorEventErgs : EventArgs
    {
        public Exception Exception { get; set; }
    }
}
