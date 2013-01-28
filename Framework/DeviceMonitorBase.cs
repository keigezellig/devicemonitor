using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NPCServiceTool.Common.Services.Logger;

namespace NPCServiceTool.Common.Services.DeviceMonitor
{
    public abstract class DeviceMonitorBase<TDeviceData, TInitParameters> : IDeviceMonitor<TDeviceData, TInitParameters> 
    {
        private CancellationTokenSource ctSource;
        private Task readerTask;
        private ILogger logger;
        protected TInitParameters parameters;

        public DeviceMonitorBase(ILogger logger)
        {
            this.logger = logger;
        }


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

        
        public void Start(TInitParameters parameters)
        {
            this.parameters = parameters;
            this.ctSource = new CancellationTokenSource();
            CancellationToken ct = ctSource.Token;

            this.readerTask = new Task(() =>
            {
                ct.ThrowIfCancellationRequested();

                try
                {
                    //Do initialization work
                    Initialize();

                    while (true)
                    {
                        if (ct.IsCancellationRequested)
                        {
                            CleanUp();
                            ct.ThrowIfCancellationRequested();
                        }

                        TDeviceData data = ReadData();
                        OnDataReceived(data);

                    }
                }
                finally
                {
                    CleanUp();
                    ct.ThrowIfCancellationRequested();

                }
            },
               ct);

            //Exceptions thrown in this task should be logged
            this.readerTask.LogExceptions(logger,ErrorEvent);
            this.readerTask.Start();


            logger.Write(Category.INFO, this.ToString(), "Start", "Read task started..");
        }

      

        public void Stop()
        {
            ctSource.Cancel();
            try
            {
                readerTask.Wait();
            }
            catch (AggregateException ex)
            {
                ex.Handle((x) =>
                {
                    if (x is TaskCanceledException)
                    {
                        logger.Write(Category.INFO, this.ToString(), "Stop", "Read task stopped..");

                    }
                    return true;
                });
            }
        }

        public event EventHandler<DataReceivedEventArgs<TDeviceData>> DataReceived;
        public event EventHandler<ErrorEventErgs> ErrorEvent;
    
       
       
        public virtual void Dispose() { }


        protected virtual void Initialize() { }
        protected abstract TDeviceData ReadData();
        protected virtual void CleanUp() { }

        protected virtual void OnDataReceived(TDeviceData data)
        {
            if (this.DataReceived != null)
            {
                var handler = this.DataReceived;
                handler(this, new DataReceivedEventArgs<TDeviceData>() { Data = data });
            }
        }






       
    }
}
