using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPCServiceTool.Common.Services.DeviceMonitor
{
    public class SignedIntegerMonitoringItem : MonitoringItemBase
    {
        public long Value { get; set; }

        public SignedIntegerMonitoringItem(string id, string description, DateTime timeStamp, long value)
            : base(id, description, timeStamp)
        {
            this.Value = value;
        }

        public SignedIntegerMonitoringItem(string id, string description, DateTime timeStamp, byte[] bytes)
            : base(id, description, timeStamp)
        {
            switch (bytes.Length)
            {
                case 1:
                    Value = (sbyte)bytes[0];
                    break;
                case 2:
                    Value = BitConverter.ToInt16(bytes, 0);
                    break;
                case 4:
                    Value = BitConverter.ToInt32(bytes, 0);
                    break;
                case 8:
                    Value = BitConverter.ToInt64(bytes, 0);
                    break;
                default:
                    throw new FormatException("Can only parse 1, 2, 4 and 8 bytes integers");
                    
            }
            


            
        }
    }
}
