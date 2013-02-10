using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using Part2.Common;

namespace Part2.Implementation
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

        public override void Dispose()
        {
            Console.WriteLine("Disposing of this junk");
            base.Dispose();
        }
    }
}
