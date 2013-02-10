using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using Part2.Common;

namespace Part2.Implementation
{
    public class TimerMonitorWithException : DeviceMonitorBase<int,DateTime>
    {
        protected override void Initialize()
        {
            Console.WriteLine("Initializing");
        }
        protected override DateTime ReadData()
        {
            //Add some artificial delay
            Thread.Sleep(1000);
            
            //Throw an exception if the seconds part is equal to the supplied value
            if (DateTime.Now.Second == this.configData)
            {
                throw new Exception("Boom!");
            }

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
