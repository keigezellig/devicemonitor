using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPCServiceTool.Common.Services.DeviceMonitor
{
    public abstract class MonitoringItemBase
    {
        public string Id { get; protected set; }
        public string Description { get; protected set; }
        public DateTime TimeStamp { get; protected set; }
        

        protected MonitoringItemBase(string id, string description, DateTime timestamp)
        {
            this.Id = id;
            this.Description = description;
            this.TimeStamp = timestamp;
        }
    }
}
