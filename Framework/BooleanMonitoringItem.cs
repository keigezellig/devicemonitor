using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPCServiceTool.Common.Services.DeviceMonitor
{
    public class BooleanMonitoringItem : MonitoringItemBase
    {
        public bool Value { get; set; }

        public BooleanMonitoringItem(string id, string description, DateTime timeStamp, bool value)
            : base(id, description, timeStamp)
        {
            this.Value = value;
        }
        
        public BooleanMonitoringItem(string id, string description, DateTime timeStamp, byte[] bytes)
            : base(id, description, timeStamp)
        {
            if (bytes.Length > 1)
            {
                throw new FormatException("Cannot parse a boolean from a multi byte array");
            }

            this.Value = BitConverter.ToBoolean(bytes, 0);
        }

        
    }
}
