using System;
using System.Threading;
using System.Threading.Tasks;

namespace Part3.Common
{
    public abstract class DeviceMonitorBase<TConfigData, TDeviceData> : IDeviceMonitor<TConfigData, TDeviceData> 
    {
        private CancellationTokenSource ctSource;
        private Task readerTask;
        protected TConfigData configData;

        //Override these methods in a derived class to implement desired functionality
        protected virtual void Initialize() { }
        protected abstract TDeviceData ReadData();
        protected virtual void CleanUp() { }
        protected virtual void Dispose(bool disposing) {}

        public event EventHandler<DataReceivedEventArgs<TDeviceData>> DataReceived;
        public event EventHandler<MonitorStatusEventArgs> MonitorStatusChanged;


        public void Start(TConfigData theConfigData)
        {
            this.configData = theConfigData;
            this.ctSource = new CancellationTokenSource();
            StartReading();
            Console.WriteLine("Monitor started..");
        }


        public void Stop()
        {
            if (this.ctSource != null)
            {
                this.ctSource.Cancel();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Read()
        {
           //Notify everyone that we started
            OnStatusChanged(MonitorStatus.STARTED);
            //Do initialization work
            Initialize();

            //This is our main loop and reads data until this task is cancelled
            while(true)
            {
                if (ctSource.Token.IsCancellationRequested)
                    ctSource.Token.ThrowIfCancellationRequested();
                //Read the data from the device
                TDeviceData data = ReadData();

                //Fire event
                OnDataReceived(data);
            }
        }


        private Task ReadAsync()
        {
            return Task.Run(() => Read(),ctSource.Token);
        }

        private async void StartReading()
        {
            try
            {
                await ReadAsync();                
            }
            catch(OperationCanceledException)
            {                
                Console.WriteLine("Cancel");
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception in task: {0}", ex);
            }
            finally
            {
                Console.WriteLine("Reader task stopped");
                CleanUp();
                OnStatusChanged(MonitorStatus.STOPPED);                
            }
        }
       
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
