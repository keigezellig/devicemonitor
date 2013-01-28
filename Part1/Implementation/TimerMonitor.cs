using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using Part1.Common;

namespace Part1.Implementation
{
    public class TimerMonitor : DeviceMonitorBase<DateTime>
    {
        protected override void Initialize()
        {
            Console.WriteLine("Initializing");
        }
        protected override DateTime ReadData()
        {
            //Add some artificial delay
            Thread.Sleep(1000);
            
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
