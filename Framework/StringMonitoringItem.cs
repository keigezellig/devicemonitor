using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPCServiceTool.Common.Services.DeviceMonitor

{
    public class StringMonitoringItem : MonitoringItemBase
    {
        public string Value { get; set; }

        public StringMonitoringItem(string id, string description, DateTime timeStamp, string value)
            : base(id, description,timeStamp)
        {
            this.Value = value;
        }

        public StringMonitoringItem(string id, string description, DateTime timeStamp, byte[] bytes, bool isUnicode)
            : base(id, description, timeStamp)
        {
            if (isUnicode)
            {
                Value = Encoding.UTF8.GetString(bytes);
            }
            else
            {
                Value = Encoding.ASCII.GetString(bytes);
            }
            
        }
    }
}
