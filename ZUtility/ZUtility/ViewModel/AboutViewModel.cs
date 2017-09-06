using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ZUtility.Command;

namespace ZUtility.ViewModel
{
    public class AboutViewModel : ViewModelBase
    {
        private string logFileName;
        private string logContent;

        public AboutViewModel()
        {
            RefreshLogContent();
            this.RefreshCommand = new RelayCommand(this.RefreshLogContent);
        }

        public string LogFileName
        {
            get { return logFileName; }
            set
            {
                if (value == logFileName)
                    return;

                logFileName = value;

                base.OnPropertyChanged("LogFileName");
            }
        }

        public string LogContent
        {
            get { return logContent; }
            set
            {
                if (value == logContent)
                    return;

                logContent = value;

                base.OnPropertyChanged("LogContent");
            }
        }
        public RelayCommand RefreshCommand
        {
            get;
            private set;
        }

        private void RefreshLogContent()
        {
            //Action action = () =>
            //{
            string logFilename = "ZUtility.log";
            using (FileStream stream = File.Open(logFilename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        String line = reader.ReadToEnd();
                        LogContent = line;
                    }
                }
            }

            string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            LogFileName = "Log: " + path + "\\" + logFilename;
            //};
            //Dispatcher.BeginInvoke(action);
        }
    }
}
