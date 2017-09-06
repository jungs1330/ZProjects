using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace DiningPhilosophers
{
    class Fork : ObservableObject
    {

        private static int idCounter = 0;
        private readonly int id;
        private readonly object syncRoot = new object();
        private string status;
        private Philosopher owner;

        public Fork()
        {
            idCounter = idCounter + 1;
            id = idCounter;
        }

        public string Status
        {
            get { return status; }
            set
            {
                status = value;
                RaisePropertyChanged("Status");
            }
        }

        public Philosopher Owner
        {
            get { return owner; }
            set
            {
                owner = value;
                RaisePropertyChanged("Owner");
            }
        }

        public override string ToString()
        {
            return Convert.ToString(id);
        }

        public bool Grab()
        {
            return Monitor.TryEnter(syncRoot);
        }

        public void Drop()
        {
            Monitor.Exit(syncRoot);
            this.Owner = null;
            this.Status = "";
        }
    }
}
