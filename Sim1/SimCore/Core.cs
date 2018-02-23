using SimShared;
using System;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace SimCore
{
    public class Core
    {
        private ManualResetEvent _mrse;
        private Thread _thread;
        public int ReplicationCount { get; }
        private Action<CoreArgs> _repFunc;
        private int _pauseDelay;

        public delegate void EvntHandler(object sender, CoreArgs e);
        public event EvntHandler OnEndEachReplication;

        public Core(int repCount, Action<CoreArgs> monteCarloFunc, int pause = 0)
        {
            _pauseDelay = pause;
            ReplicationCount = repCount;
            _repFunc = monteCarloFunc;
            _mrse = new ManualResetEvent(initialState: true);
            _thread = new Thread(new ThreadStart(DoReplicationWrapper));
            _thread.Priority = ThreadPriority.BelowNormal;
        }

        public void Stop()
        {
            _thread.Abort();
        }

        public void Start()
        {
            _thread.Start();
        }

        public void Pause()
        {
            _mrse.Reset();
        }

        public void Resume()
        {
            _mrse.Set();
        }

        private void DoReplicationWrapper()
        {
            for (int i = 0; i < ReplicationCount; i++)
            {
                var arg = new CoreArgs(i);
                _repFunc(arg);

                if (_pauseDelay > 0)
                {
                    Thread.Sleep(_pauseDelay);
                }

                OnEndEachReplication?.Invoke(this, arg);
                _mrse.WaitOne();
            }
        }
        
    }

}