using SimCore;
using SimShared;
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
        private Core _coreDontChange;
        private Core _coreChanged;

        public delegate void EvntHandler(object sender, CoreArgs e);

        public event EvntHandler OnDontChangedDecision;
        public event EvntHandler OnChangedDecision;

        private int _doors;
        private int _rep;
        private Random _dRnd;
        private Random _cRnd;

        private int _succesAllNotChange;
        private int _succesAllChange;

        public MainLog(int doors, int replications) {
            if (doors < 3) {
                throw new ArgumentException("Počet dverí musí byť väčši ako 3!");
            }
            _dRnd = new Random();
            _cRnd = new Random();
            _succesAllChange = 0;
            _succesAllNotChange = 0;
            
            _doors = doors;
            _rep = replications;

            _coreDontChange = new Core(_rep, (x) => DontChangeDecision(x));
            _coreDontChange.OnEndEachReplication += new Core.EvntHandler(UpdateStatusDontChange);
            
            _coreChanged = new Core(_rep, (x) => ChangedDecision(x));
            _coreChanged.OnEndEachReplication += new Core.EvntHandler(UpdateStatusChanged);
        }

        private void UpdateStatusDontChange(object o, CoreArgs e) {
            OnDontChangedDecision?.Invoke(o,e);
        }

        public void Start()
        {
            _coreChanged.Start();
            _coreDontChange.Start();
        }

        private void UpdateStatusChanged(object o, CoreArgs e) {
            OnChangedDecision?.Invoke(o, e);
        }

        private void DontChangeDecision(CoreArgs arg)
        {
            var carIdx = _dRnd.Next(_doors);
            var selectIdx = _dRnd.Next(_doors);
            if (carIdx == selectIdx)
            {
                arg.Successful = true;
                _succesAllNotChange++;
            }

            arg.AllSucces = _succesAllNotChange;

        }

        private void ChangedDecision(CoreArgs arg)
        {
            var carIdx = _dRnd.Next(_doors);
            var openIdx = _dRnd.Next(_doors - 1);
            var selIdx = _dRnd.Next(_doors - 1);

            openIdx = (carIdx <= openIdx) ? (openIdx + 1) : openIdx;
            selIdx = (openIdx <= selIdx) ? (selIdx + 1) : openIdx;

            if (carIdx == selIdx)
            {
                arg.Successful = true;
                _succesAllNotChange++;
            }

            arg.AllSucces = _succesAllChange;
        }

        public void Stop()
        {
            _coreChanged.Stop();
            _coreDontChange.Stop();
        }
    }
}
