using SimCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimLog
{
    public class MainLog
    {
        private Core _core;

        public MainLog() {

            _core = new Core(10000, (x) => {
                Thread.Sleep(500);
                Console.WriteLine($"aaa {x}");
            });
            _core.OnEndEachReplication += new Core.EvntHandler(UpdateStatus);
        }

        
    }
}
