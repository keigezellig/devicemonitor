using System;
using System.Threading;
using Part3.Common;

namespace Part3.Implementation
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Console.WriteLine("Disposing managed resources");
            }
            base.Dispose(disposing);
        }
    }
}
