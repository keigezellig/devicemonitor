using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPCServiceTool.Common.Services.DeviceMonitor
{
    public class UnsignedIntegerMonitoringItem : MonitoringItemBase
    {
        public ulong Value { get; set; }

        public UnsignedIntegerMonitoringItem(string id, string description, DateTime timeStamp, ulong value)
            : base(id, description,timeStamp)
        {
            this.Value = value;
        }

        public UnsignedIntegerMonitoringItem(string id, string description, DateTime timeStamp, byte[] bytes)
            : base(id, description, timeStamp)
        {
            switch (bytes.Length)
            {
                case 1:
                    Value = bytes[0];
                    break;
                case 2:
                    Value = BitConverter.ToUInt16(bytes, 0);
                    break;
                case 4:
                    Value = BitConverter.ToUInt32(bytes, 0);
                    break;
                case 8:
                    Value = BitConverter.ToUInt64(bytes, 0);
                    break;
                default:
                    throw new FormatException("Can only parse 1, 2, 4 and 8 bytes integers");

            }
        }
    }
}
