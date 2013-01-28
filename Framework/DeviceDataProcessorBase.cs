using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPCServiceTool.Common.Services.DeviceMonitor
{
    public abstract class DeviceDataProcessorBase<TDeviceData, TProcessedData> : IDeviceDataProcessor<TDeviceData, TProcessedData>
    {
        
        public bool Handled { get; private set; }

        public TProcessedData StartProcessing(TDeviceData data)
        {
            try
            {
                if (CanHandle(data))
                {
                    TProcessedData result = ProcessData(data);
                    Handled = true;
                    return result;
                }
                else
                {
                    Handled = false;
                }
            }
            catch (Exception ex)
            {
                Handled = true;
                throw;
            }

            return default(TProcessedData);
            
            
        }

 
        protected abstract TProcessedData ProcessData(TDeviceData data);
        protected abstract bool CanHandle(TDeviceData data);
        

     
    }
}
