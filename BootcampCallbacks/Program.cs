using System;
using System.Threading;

namespace BootcampCallbacks
{
    class Program
    {
        private string message;
        //[Flags]
        private enum taskStati
        {
            Processing,Completed
        };

        private taskStati taskStatus;
        private static Timer timer;
        private static bool complete;
        static void Main(string[] args)
        {
            Program p = new Program();
            Thread workerThread = new Thread(p.DoSomeWork);
            workerThread.Start();

            // create timer with callback
            TimerCallback timerCallBack = new TimerCallback(p.GetState);
            timer = new Timer(timerCallBack, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(100));

            // wait for worker to complete
            do
            {
                // simply wait, do nothing
            } while (!complete);

            Console.WriteLine("exiting main thread");
            Console.ReadKey();
        }

        public void GetState(object state)
        {
            message = taskStatus.ToString();
            // not done so return
            if (message == string.Empty) return;
            Console.WriteLine("Work is {0}", message);

            // is other thread completed yet, if so signal main thread to stop waiting
            if(message=="Completed")
            {
                timer.Dispose();
                complete = true;
            }
        }

        public void DoSomeWork()
        {
            //message = "processing";
            taskStatus = taskStati.Processing;
            //simulate doing some work
            Thread.Sleep(3000);
            taskStatus = taskStati.Completed;
            //message = "Completed";
        }
    }
}
