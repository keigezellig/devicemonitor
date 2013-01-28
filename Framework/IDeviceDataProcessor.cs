using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPCServiceTool.Common.Services.DeviceMonitor
{
    public interface IDeviceDataProcessor<TDeviceData, TProcessedData> 
    {
        TProcessedData StartProcessing(TDeviceData data);
        bool Handled { get; }
    }
   
}
