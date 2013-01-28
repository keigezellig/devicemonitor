using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Part2.Implementation;
using Part2.Common;


namespace Part1
{
    class Program
    {
        public static IDeviceMonitor<DateTime> monitor = null;
        public static MonitorStatus currentStatus;

        static void Main(string[] args)
        {
            

            Console.WriteLine("m = Start monitor");
            Console.WriteLine("e = Start monitor with exception");
            Console.WriteLine("s = Stop current monitor");
            Console.WriteLine("q = Quit program");

            ConsoleKeyInfo key = default(ConsoleKeyInfo);

            while (key.KeyChar != 'q')
            {
                key = Console.ReadKey();

                if (key.KeyChar == 'm')
                {
                    if (monitor == null || currentStatus == MonitorStatus.STOPPED)
                    {
                        Console.WriteLine("Starting monitor");
                        monitor = new TimerMonitor();
                        monitor.DataReceived += new EventHandler<DataReceivedEventArgs<DateTime>>(monitor_DataReceived);
                        monitor.MonitorStatusChanged += new EventHandler<MonitorStatusEventArgs>(monitor_MonitorStatusChanged);
                        monitor.Start();
                    }
 
                }
                if (key.KeyChar == 'e')
                {
                    if (monitor == null || currentStatus == MonitorStatus.STOPPED)
                    {
                        Console.WriteLine("Starting monitor with exception");
                        monitor = new TimerMonitorWithException();
                        monitor.MonitorStatusChanged += new EventHandler<MonitorStatusEventArgs>(monitor_MonitorStatusChanged);
                        monitor.DataReceived += new EventHandler<DataReceivedEventArgs<DateTime>>(monitor_DataReceived);
                        monitor.Start();
                    }

                }

                if (key.KeyChar == 's')
                {
                    if (monitor != null && currentStatus == MonitorStatus.STARTED)
                    {
                        monitor.Stop();
                       
                    }
                }
            }


            if (monitor != null && currentStatus == MonitorStatus.STARTED)
            {
                monitor.Stop();
                monitor.Dispose();
            }
        }

        static void monitor_MonitorStatusChanged(object sender, MonitorStatusEventArgs e)
        {
            currentStatus = e.Status;
        }

        static void monitor_DataReceived(object sender, DataReceivedEventArgs<DateTime> e)
        {
            Console.WriteLine("Data received! Data = " + e.Data.ToString());
        }
    }
}
