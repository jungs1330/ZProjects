using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using ZUtility.Properties;
using ZUtility.Command;

namespace ZUtility.ViewModel
{
    public class ThreadingViewModel : WorkspaceViewModel
    {
        private string input;
        private string output;
        private string threadMethod;
        private bool isStartEnabled;
        private bool isStopEnabled;
        delegate string Del(ThreadingViewModel tvm);
        private List<string> threadMethods = new List<string>() { "Asynchronous Delegate", "Thread", "Thread Pool", "Background Workder", "Task", "Await Task" };
        BackgroundWorker bw;
        CancellationTokenSource cancelSource = new CancellationTokenSource();
        
        public ThreadingViewModel()
        {
            base.DisplayName = Resources.MainWindowViewModel_Command_Threading;

            this.StartCommand = new RelayCommand(this.Start, this.IsStartEnabled);
            this.StopCommand = new RelayCommand(this.Stop, this.IsStopEnabled);
            this.ClearCommand = new RelayCommand(this.Clear, null);
            isStartEnabled = true;
            isStopEnabled = true;
            this.Output = "Ready........";
        }

        public List<string> ThreadMethods
        { get { return threadMethods; } }

        public string Input
        {
            get { return input; }
            set
            {
                if (value == input)
                    return;

                input = value;

                base.OnPropertyChanged("Input");
            }
        }

        public string Output
        {
            get { return output; }
            set
            {
                if (value == output)
                    return;

                output = value;

                base.OnPropertyChanged("Output");
            }
        }

        public string ThreadMethod
        {
            get { return threadMethod; }
            set
            {
                if (value == threadMethod)
                    return;

                threadMethod = value;

                base.OnPropertyChanged("ThreadMethod");
            }
        }

        private bool IsStartEnabled()
        {
            return isStartEnabled;
        }

        private bool IsStopEnabled()
        {
            return isStopEnabled;
        }

        public RelayCommand StartCommand
        {
            get;
            private set;
        }

        public RelayCommand StopCommand
        {
            get;
            private set;
        }

        public RelayCommand ClearCommand
        {
            get;
            private set;
        }

        private void Clear()
        {
            this.Output = "";
        }

        private void Start()
        {
            if (string.IsNullOrWhiteSpace(this.Input))
            {
                this.Output = this.Output + "\r\n" + GetThreadID() + "Input required!";
                return;
            }

            isStartEnabled = false;
            isStopEnabled = true;
            this.Output = this.Output + "\r\n" + GetThreadID() + "Button Started.....";
            
            switch (ThreadMethod)
            {
                case "Asynchronous Delegate":
                    DoWorkDelegate();
                    break;
                case "Thread":
                    DoWorkThread();
                    break;
                case "Thread Pool":
                    DoWorkThreadPool();
                    break;
                case "Background Workder":
                    DoWorkBackgroundWorkder();
                    break;
                case "Task":
                    DoWorkTask();
                    break;
                case "Await Task":
                    DoWorkAwaitTask(cancelSource.Token);
                    break;
            }

            this.Output = this.Output + "\r\n" + GetThreadID() + "Button Finished.....";
            isStartEnabled = true;
            //isStopEnabled = false;
        }

        private void Stop()
        {
            isStartEnabled = true;
            isStopEnabled = false;

            if (bw != null && bw.IsBusy)
            {
                bw.CancelAsync();
            }
            else if (cancelSource.Token != null)
            {
                cancelSource.Cancel();
            }
            else
            {
                this.Output = this.Output + "\r\nNothing to stop.....";
            }
        }
        private void DoWorkDelegate()
        {
            this.Output = this.Output + "\r\n" + GetThreadID() + "Delegate started";

            Del d = new Del(BackGroundTask2);
            IAsyncResult result = d.BeginInvoke(this, new AsyncCallback(AsyncDelegateCallback), null);
        }

        private void DoWorkThread()
        {
            Thread thread = new Thread(new ParameterizedThreadStart(BackGroundTask1));
            thread.Name = "My Thread";
            thread.Priority = ThreadPriority.Normal;
            thread.Start(this);
        }

        private void DoWorkThreadPool()
        {
            WaitCallback workItem = new WaitCallback(BackGroundTask3);
            ThreadPool.QueueUserWorkItem(workItem, this);
        }

        private void DoWorkTask()
        {
            //using lamda expression instead of Action delegate.
            //Task task = Task.Factory.StartNew(()=>BackGroundTask4(this));

            //using Action delegate
            Task task = Task.Factory.StartNew(new Action<object>(BackGroundTask4), this);

            this.Output = this.Output + "\r\n" + GetThreadID() + "DoWorkTask Completed Before";

            TaskAwaiter awaiter = task.GetAwaiter();
            awaiter.OnCompleted(
                () => { this.Output = this.Output + "\r\n" + GetThreadID() + "DoWorkTask Completed."; }
                );

            this.Output = this.Output + "\r\n" + GetThreadID() + "DoWorkTask Completed After";
        }

        private async void DoWorkAwaitTask(CancellationToken cancellationToken)
        {
            this.Output = this.Output + "\r\n" + GetThreadID() + "DoWorkAwaitTask Completed Before";

            cancellationToken.Register(() => { this.Output = this.Output + "\r\n" + GetThreadID() + "DoWorkAwaitTask Cancelled"; });
            
            await BackGroundTask5(this, cancellationToken);
            
            this.Output = this.Output + "\r\n" + GetThreadID() + "DoWorkAwaitTask Completed After";
        }

        private void DoWorkBackgroundWorkder()
        {
            bw = new BackgroundWorker();

            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;

            bw.RunWorkerAsync();
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            // The sender is the BackgroundWorker object we need it to
            // report progress and check for cancellation.
            //NOTE : Never play with the UI thread here...
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(1000);

                // Periodically report progress to the main thread so that it can
                // update the UI.  In most cases you'll just need to send an
                // integer that will update a ProgressBar                    
                bw.ReportProgress(i * 10);
                // Periodically check if a cancellation request is pending.
                // If the user clicks cancel the line
                // m_AsyncWorker.CancelAsync(); if ran above.  This
                // sets the CancellationPending to true.
                // You must check this flag in here and react to it.
                // We react to it by setting e.Cancel to true and leaving
                if (bw.CancellationPending)
                {
                    // Set the e.Cancel flag so that the WorkerCompleted event
                    // knows that the process was cancelled.
                    e.Cancel = true;
                    bw.ReportProgress(0);
                    return;
                }
            }

            //Report 100% completion on operation completed
            bw.ReportProgress(100);
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            // This function fires on the UI thread so it's safe to edit
            // the UI control directly, no funny business with Control.Invoke :)
            this.Output = this.Output + "\r\n" + GetThreadID() + "Background worker processing......" + e.ProgressPercentage + "%";
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // The background process is complete. We need to inspect
            // our response to see if an error occurred, a cancel was
            // requested or if we completed successfully.  
            if (e.Cancelled)
            {
                this.Output = this.Output + "\r\n" + GetThreadID() + "Background worker cancelled.";
            }

            // Check to see if an error occurred in the background process.
            else if (e.Error != null)
            {
                this.Output = this.Output + "\r\n" + GetThreadID() + "Error while performing background operation.";
            }
            else
            {
                this.Output = this.Output + "\r\n" + GetThreadID() + "Background workder completed.";
                // Everything completed normally.
            }
        }

        private static void BackGroundTask1(object vm)
        {
            ThreadingViewModel tvm = vm as ThreadingViewModel;
            int _counter = int.Parse(tvm.Input.ToString());

            tvm.Output = tvm.Output + "\r\n" + GetThreadID() + "Thread starting.....";

            for (int i = 0; i < _counter; i++)
            {
                Thread.Sleep(1000);
                tvm.Output = tvm.Output + "\r\n" + GetThreadID() + "Thread running.....";
            }

            tvm.Output = tvm.Output + "\r\n" + GetThreadID() + "Thread finished.....";
        }

        private static string BackGroundTask2(ThreadingViewModel tvm)
        {
            int counter = int.Parse(tvm.Input.ToString());
            for (int i = 0; i < counter; i++)
            {
                Thread.Sleep(1000);
                tvm.Output = tvm.Output + "\r\n" + GetThreadID() + "Delegate running.....";
            }
            return "Delegate Done.";
        }

        private static void BackGroundTask3(object vm)
        {
            ThreadingViewModel tvm = vm as ThreadingViewModel;
            int _counter = int.Parse(tvm.Input.ToString());

            tvm.Output = tvm.Output + "\r\n" + GetThreadID() + "ThreadPool starting.....";

            for (int i = 0; i < _counter; i++)
            {
                Thread.Sleep(1000);
                tvm.Output = tvm.Output + "\r\n" + GetThreadID() + "ThreadPool running.....";
            }

            tvm.Output = tvm.Output + "\r\n" + GetThreadID() + "ThreadPool finished.....";
        }

        private static void BackGroundTask4(object vm)
        {
            ThreadingViewModel tvm = vm as ThreadingViewModel;
            int _counter = int.Parse(tvm.Input.ToString());

            tvm.Output = tvm.Output + "\r\n" + GetThreadID() + "Task starting.....";

            for (int i = 0; i < _counter; i++)
            {
                Thread.Sleep(1000);
                tvm.Output = tvm.Output + "\r\n" + GetThreadID() + "Task running.....";
            }

            tvm.Output = tvm.Output + "\r\n" + GetThreadID() + "Task finished.....";
        }

        private static Task BackGroundTask5(object vm, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
                {
                    ThreadingViewModel tvm = vm as ThreadingViewModel;
                    int _counter = int.Parse(tvm.Input.ToString());

                    tvm.Output = tvm.Output + "\r\n" + GetThreadID() + "Await Task starting.....";

                    for (int i = 0; i < _counter; i++)
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            tvm.Output = tvm.Output + "\r\n" + GetThreadID() + "Await Task cancelling.....";
                            return;
                        }
                        Thread.Sleep(1000);
                        tvm.Output = tvm.Output + "\r\n" + GetThreadID() + "Await Task running.....";
                    }

                    tvm.Output = tvm.Output + "\r\n" + GetThreadID() + "Await Task finished.....";
                });
        }

        private void AsyncDelegateCallback(IAsyncResult ar)
        {
            this.Output = this.Output + "\r\n" + GetThreadID() + "Delegate finished.....";
        }

        private static string GetThreadID()
        {
            return "Thread[" + Thread.CurrentThread.ManagedThreadId + "]";
        }
    }
}
