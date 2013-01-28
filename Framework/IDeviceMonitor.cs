using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPCServiceTool.Common.Services.DeviceMonitor
{
    public interface IDeviceMonitor<TDeviceData, TInitParameters> : IDisposable 
    {
        void Start(TInitParameters parameters);
        void Stop();
        MonitorStatus MonitorStatus { get; }

        event EventHandler<DataReceivedEventArgs<TDeviceData>> DataReceived;
        event EventHandler<ErrorEventErgs> ErrorEvent;
    }


    public class DataReceivedEventArgs<TTDeviceData> : EventArgs
    {
        public TTDeviceData Data { get; set; }
    }

    public enum MonitorStatus
    {
        STARTED, STOPPED
    }
}
