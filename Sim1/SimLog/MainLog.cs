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

        public MainLog(int doors, int replications,int pause) {
            if (doors < 3) {
                throw new ArgumentException("Počet dverí musí byť väčši ako 3!");
            }
            _dRnd = new Random();
            _cRnd = new Random();
            _succesAllChange = 0;
            _succesAllNotChange = 0;
                        
            _doors = doors;
            _rep = replications;

            _coreDontChange = new Core(_rep, (x) => DontChangeDecision(x),pause);
            _coreDontChange.OnEndEachReplication += new Core.EvntHandler(UpdateStatusDontChange);
            
            _coreChanged = new Core(_rep, (x) => ChangedDecision(x),pause);
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
            var carIdx = _cRnd.Next(_doors);
            var sel1Idx = _cRnd.Next(_doors);
            var openIdx = _cRnd.Next(_doors - 2);
            var sel2Idx = _cRnd.Next(_doors - 2);

            // otvorim dvere kde nie je auto a zaroven ktore neotvoril sutaziaci
            openIdx = (Math.Min(carIdx,sel1Idx) <= openIdx) ? (openIdx + 1) : openIdx;
            openIdx = (Math.Max(carIdx, sel1Idx) <= openIdx) ? (openIdx + 1) : openIdx;

            //vyberam dvere ktore nie su otvorene a zaroven nie su tie co som ako prve otvoril
            sel2Idx = (Math.Min(sel1Idx, openIdx) <= sel2Idx) ? (sel2Idx + 1) : sel2Idx;
            sel2Idx = (Math.Max(sel1Idx, openIdx) <= sel2Idx) ? (sel2Idx + 1) : sel2Idx;

            if (carIdx == sel2Idx)
            {
                arg.Successful = true;
                _succesAllChange++;
            }

            arg.AllSucces = _succesAllChange;
        }

        public void Stop()
        {
            _coreDontChange.OnEndEachReplication -= UpdateStatusDontChange;
            _coreChanged.OnEndEachReplication -= UpdateStatusChanged;
            _coreChanged.Stop();
            _coreDontChange.Stop();

            _succesAllNotChange = 0;
            _succesAllChange = 0;
        }

        public void Pause()
        {
            _coreChanged.Pause();
            _coreDontChange.Pause();
        }

        public void Continue()
        {
            _coreChanged.Resume();
            _coreDontChange.Resume();
        }
    }
}
