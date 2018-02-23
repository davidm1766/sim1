using System;
using System.Threading;


namespace SimCore
{
    public class Core
    {
        private ManualResetEvent _mrse;
        private Thread _thread;
        public int ReplicationCount { get; }
        private Action<int> _repFunc;

        public delegate void EvntHandler(object sender, CoreArgs e);
        public event EvntHandler OnEndEachReplication;

        public Core(int repCount, Action<int> monteCarloFunc)
        {
            ReplicationCount = repCount;
            _repFunc = monteCarloFunc;
            _mrse = new ManualResetEvent(initialState: true);
            _thread = new Thread(new ThreadStart(DoReplicationWrapper));
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
                _repFunc(i);
                OnEndEachReplication?.Invoke(this, new CoreArgs(i));
                _mrse.WaitOne();
            }
        }

    }

}
public class CoreArgs : EventArgs
{
    public int Iteration { get; }

    public CoreArgs(int interation)
    {
        Iteration = interation;
    }
}