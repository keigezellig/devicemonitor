using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPCServiceTool.Common.Services.DeviceMonitor
{
    public class DoubleMonitoringItem : MonitoringItemBase
    {
        public double Value { get; set; }

        public DoubleMonitoringItem(string id, string description, DateTime timeStamp, double value)
            : base(id, description, timeStamp)
        {
            this.Value = value;
        }

        public DoubleMonitoringItem(string id, string description, DateTime timeStamp, byte[] bytes)
            : base(id, description, timeStamp)
        {
            switch (bytes.Length)
            {
                case 4:
                    Value = BitConverter.ToSingle(bytes, 0);
                    break;
                case 8:
                    Value = BitConverter.ToDouble(bytes, 0);
                    break;
                default:
                    throw new FormatException("Can only parse 4 and 8 bytes floats");

            }
        }
    }
}
