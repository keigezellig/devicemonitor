using System;
using System.Threading;
using Part3.Common;

namespace Part3.Implementation
{
    public class TimerMonitor : DeviceMonitorBase<int,DateTime>
    {
        protected override void Initialize()
        {
            Console.WriteLine("Initializing");
        }
        protected override DateTime ReadData()
        {
            //Set the refresh rate to the supplied value
            Thread.Sleep(this.configData);
            
            //Just return the current time
            return DateTime.Now;
             
            
        }
        protected override void CleanUp()
        {
            Console.WriteLine("Cleaning up");
        }

       
        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                Console.WriteLine("Disposing managed resources");
            }
            base.Dispose(disposing);
        }
    }
}
