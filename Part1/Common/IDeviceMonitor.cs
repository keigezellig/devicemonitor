using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Part1.Common
{
    public interface IDeviceMonitor<TDeviceData> : IDisposable 
    {
        void Start();
        void Stop();
        event EventHandler<DataReceivedEventArgs<TDeviceData>> DataReceived;
        event EventHandler<MonitorStatusEventArgs> MonitorStatusChanged;
    }


    public class DataReceivedEventArgs<TTDeviceData> : EventArgs
    {
        public TTDeviceData Data { get; set; }
    }

    public class MonitorStatusEventArgs : EventArgs
    {
        public MonitorStatus Status { get; set; }
    }

   

    public enum MonitorStatus
    {
        STARTED, STOPPED
    }
}
