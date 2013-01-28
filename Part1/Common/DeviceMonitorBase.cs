using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Part1.Common
{
    public abstract class DeviceMonitorBase<TDeviceData> : IDeviceMonitor<TDeviceData> 
    {
        private CancellationTokenSource ctSource;
        private Task readerTask;
    
        public MonitorStatus MonitorStatus
        {
            get
            {
                if (this.readerTask != null && this.readerTask.Status == TaskStatus.Running)
                {
                    return MonitorStatus.STARTED;
                }

                return MonitorStatus.STOPPED;
            }
        }


        public void Start()
        {

            this.ctSource = new CancellationTokenSource();
            CancellationToken ct = ctSource.Token;

            this.readerTask = new Task(() =>
            {
                //Are we cancelled yet?
                ct.ThrowIfCancellationRequested();

                //Notify everyone that we started
                OnStatusChanged(MonitorStatus.STARTED);
                
                //Do initialization work
                Initialize();

                //This is our main loop and reads data until this task is cancelled
                while (true)
                {
                    if (ct.IsCancellationRequested)
                    {
                        //Cancel it
                        CleanUp();
                        ct.ThrowIfCancellationRequested();
                    }

                    //Read the data from the device
                    TDeviceData data = ReadData();

                    //Fire event
                    OnDataReceived(data);
                }

            }, ct);

            //After the task is cancelled, start a continuation task which prints in this case a message that the task is cancelled
            //(or actually stopped in our case)
            this.readerTask.ContinueWith(t =>
                {
                    Console.WriteLine("Read task stopped");
                    OnStatusChanged(MonitorStatus.STOPPED);
                }, TaskContinuationOptions.OnlyOnCanceled);
               
            this.readerTask.Start();
            Console.WriteLine("Read task started..");
        }


        public void Stop()
        {
            ctSource.Cancel();
        }

        public event EventHandler<DataReceivedEventArgs<TDeviceData>> DataReceived;
        public event EventHandler<MonitorStatusEventArgs> MonitorStatusChanged;
    
        //Override these methods in a derived class to implement desired functionality
        protected virtual void Initialize() { }
        protected abstract TDeviceData ReadData();
        protected virtual void CleanUp() { }
        public virtual void Dispose() { }
       
        protected virtual void OnDataReceived(TDeviceData data)
        {
            if (this.DataReceived != null)
            {
                var handler = this.DataReceived;
                handler(this, new DataReceivedEventArgs<TDeviceData>() { Data = data });
            }
        }

        protected virtual void OnStatusChanged(MonitorStatus newStatus)
        {
            if (this.MonitorStatusChanged != null)
            {
                var handler = this.MonitorStatusChanged;
                handler(this, new MonitorStatusEventArgs() { Status = newStatus });
            }
        }






       
    }
}
