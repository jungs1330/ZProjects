using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiningPhilosophers
{
    class Philosopher : ObservableObject
    {
        private readonly string name;
        private readonly Fork left;
        private readonly Fork right;
        private bool isDining;
        private string status;
        private int eatCount = 0;
        private int thinkingTime = 1000;

        public int ThinkingTime
        {
            get { return thinkingTime; }
            set
            {
                thinkingTime = value;
                RaisePropertyChanged("ThinkingTime");
            }
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

        public string Name
        {
            get { return name; }
        }

        public int EatCount
        {
            get { return eatCount; }
            set
            {
                eatCount = value;
                RaisePropertyChanged("EatCount");
            }
        }

        public Philosopher(string name, Fork left, Fork right)
        {
            this.name = name;
            this.left = left;
            this.right = right;
        }

        public void Dine()
        {
            while (isDining)
            {
                this.Status = "Try get left fork.";
                if (left.Grab())
                {
                    try
                    {
                        left.Owner = this;
                        left.Status = "In use";
                        this.Status = "Get left fork.";
                        this.Status = "Try get right fork.";

                        if (right.Grab())
                        {
                            try
                            {
                                right.Owner = this;
                                right.Status = "In use";
                                this.EatCount += 1;
                                this.Status = "Eating and thinking";
                                System.Threading.Thread.Sleep(this.ThinkingTime);
                            }
                            finally
                            {
                                right.Drop();
                                this.Status = "Dropped both forks.";
                            }
                        }
                    }
                    finally
                    {
                        left.Drop();
                    }
                }
            }
        }

        public Task Start()
        {
            if (isDining)
                throw new InvalidOperationException("Cannot start dining when already dining.");

            isDining = true;

            return Task.Factory.StartNew(Dine);
        }

        public void StopDining()
        {
            if (!isDining)
                throw new InvalidOperationException("Cannot stop dining when not already dining.");

            isDining = false;
        }
    }
}
