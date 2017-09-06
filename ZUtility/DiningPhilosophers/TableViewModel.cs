using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiningPhilosophers
{
    class TableViewModel : ObservableObject
    {
        private string philosopherThinkingTime;
        private Fork fork1;
        private Fork fork2;
        private Fork fork3;
        private Fork fork4;
        private Fork fork5;
        private Philosopher philosopher1;
        private Philosopher philosopher2;
        private Philosopher philosopher3;
        private Philosopher philosopher4;
        private Philosopher philosopher5;
        private bool isRunning = false;
        
        #region Constructor
        public TableViewModel()
        {
            PhilosopherThinkingTime = "2000";

            fork1 = new Fork();
            fork2 = new Fork();
            fork3 = new Fork();
            fork4 = new Fork();
            fork5 = new Fork();

            philosopher1 = new Philosopher("1", fork1, fork2);
            philosopher2 = new Philosopher("2", fork2, fork3);
            philosopher3 = new Philosopher("3", fork3, fork4);
            philosopher4 = new Philosopher("4", fork4, fork5);
            philosopher5 = new Philosopher("5", fork5, fork1);

            this.RunCommand = new RelayCommand(this.OnRun, this.CanRun);
            this.StopCommand = new RelayCommand(this.OnStop, this.CanStop);
        }
        #endregion

        #region Properties

        public RelayCommand RunCommand
        {
            get;
            private set;
        }

        public RelayCommand StopCommand
        {
            get;
            private set;
        }

        public bool CanRun()
        {
            return !isRunning;
        }

        public bool CanStop()
        {
            return isRunning;
        }

        public string PhilosopherThinkingTime
        {
            get { return philosopherThinkingTime; }
            set
            {
                philosopherThinkingTime = value;
                RaisePropertyChanged("PhilosopherThinkingTime");
            }
        }

        public Fork Fork1
        {
            get { return fork1; }
            set
            {
                fork1 = value;
                RaisePropertyChanged("Fork1");
            }
        }

        public Fork Fork2
        {
            get { return fork2; }
            set
            {
                fork2 = value;
                RaisePropertyChanged("Fork2");
            }
        }

        public Fork Fork3
        {
            get { return fork3; }
            set
            {
                fork3 = value;
                RaisePropertyChanged("Fork3");
            }
        }

        public Fork Fork4
        {
            get { return fork4; }
            set
            {
                fork4 = value;
                RaisePropertyChanged("Fork4");
            }
        }

        public Fork Fork5
        {
            get { return fork5; }
            set
            {
                fork5 = value;
                RaisePropertyChanged("Fork5");
            }
        }

        public Philosopher Philosopher1
        {
            get { return philosopher1; }
            set
            {
                philosopher1 = value;
                RaisePropertyChanged("Philosopher1");
            }
        }

        public Philosopher Philosopher2
        {
            get { return philosopher2; }
            set
            {
                philosopher2 = value;
                RaisePropertyChanged("Philosopher2");
            }
        }

        public Philosopher Philosopher3
        {
            get { return philosopher3; }
            set
            {
                philosopher3 = value;
                RaisePropertyChanged("Philosopher3");
            }
        }

        public Philosopher Philosopher4
        {
            get { return philosopher4; }
            set
            {
                philosopher4 = value;
                RaisePropertyChanged("Philosopher4");
            }
        }

        public Philosopher Philosopher5
        {
            get { return philosopher5; }
            set
            {
                philosopher5 = value;
                RaisePropertyChanged("Philosopher5");
            }
        }

        #endregion

        private void OnRun()
        {
            this.isRunning = true;
            this.RunCommand.RaiseCanExecuteChanged();
            this.StopCommand.RaiseCanExecuteChanged();

            philosopher1.ThinkingTime = int.Parse(PhilosopherThinkingTime);
            philosopher2.ThinkingTime = int.Parse(PhilosopherThinkingTime);
            philosopher3.ThinkingTime = int.Parse(PhilosopherThinkingTime);
            philosopher4.ThinkingTime = int.Parse(PhilosopherThinkingTime);
            philosopher5.ThinkingTime = int.Parse(PhilosopherThinkingTime);

            philosopher1.Start();
            philosopher2.Start();
            philosopher3.Start();
            philosopher4.Start();
            philosopher5.Start();
        }

        private void OnStop()
        {
            philosopher1.StopDining();
            philosopher2.StopDining();
            philosopher3.StopDining();
            philosopher4.StopDining();
            philosopher5.StopDining();

            this.isRunning = false;
            this.RunCommand.RaiseCanExecuteChanged();
            this.StopCommand.RaiseCanExecuteChanged();
        }
    }
}
