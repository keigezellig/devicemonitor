using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Part2.Common
{
    public abstract class DeviceMonitorBase<TDeviceData> : IDeviceMonitor<TDeviceData> 
    {
        private CancellationTokenSource ctSource;
        private Task readerTask;
    
        


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

            },
               ct);
               
              //Extend the continuation task so that it handles exceptions as well. You can for example write them to a log file
               this.readerTask.ContinueWith(t => 
               {
                   if (t.IsFaulted)
                   {
                       t.Exception.Handle((x) =>
                       {
                           Console.WriteLine("Exception in task: {0}", x);
                           return true;
                       });

                       try
                       {
                           CleanUp();
                       }
                       catch (Exception ex)
                       {
                           Console.WriteLine("Cleanup exception: {0}", ex.Message);
                       }
                   }

                   //Notify everyone that we stopped
                   OnStatusChanged(MonitorStatus.STOPPED); 
                   Console.WriteLine("Reader task stopped");
               },TaskContinuationOptions.NotOnRanToCompletion);

          

            this.readerTask.Start();
            Console.WriteLine("Read task started..");
        }

      

        public void Stop()
        {
           //In case of an exception thrown in the task (then it's in the Faulted state) the task has already been stopped
            if (!this.readerTask.IsFaulted)
            {
                ctSource.Cancel();
            }
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
