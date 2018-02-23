using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimShared
{

    public class CoreArgs : EventArgs
    {
        public int Iteration { get; }

        public bool Successful { get; set; }

        public int AllSucces { get; set; }

        public double PercentOfSuccessfull
        {
            get
            {
                return (double)AllSucces / (Iteration + 1);
            }
        }

        public CoreArgs(int interation)
        {
            Iteration = interation;
            Successful = false;
            AllSucces = 0;
        }
    }
}
