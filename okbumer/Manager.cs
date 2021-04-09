using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace okbumer
{
    class Manager
    {
        private static Form1 _mainForm;
        private static Job _mainJob;
        private static Task<bool> _mainTask;

        public Manager(Form1 frm) {
            _mainForm = frm;
        }

        public void FoundWinner(int winner)
        {
            if (_mainJob != null) {
                _mainJob.SetWinner(winner);
                if (winner != 0) {
                    _mainJob.Reset(true);
                }
            }
        }

        public void ResetJob()
        {
            if (_mainJob != null)
            {
                _mainJob.Reset(false);
            }
        }

        public async Task StartAsync()
        {
            var progress = new Progress<int>(v => _mainForm.UpdateProgress(v));
            await Start(progress);
        }

        private Task<bool> Start(IProgress<int> progress)
        {

            if (_mainTask == null || _mainTask.Status != TaskStatus.Running)
            {
                _mainTask = Task.Run(() =>
                    {
                        _mainJob = new Job();
                        _mainJob.Start(progress);
                        return true;
                    });
            }
            return _mainTask;
        }
    }

    class Job
    {
        private static int _hasWinner = 0;
        private static bool _reset = false;
        public void Start(IProgress<int> progress)
        {
            int step = 0;
            while (!_reset)
            {
                Thread.Sleep(125);
                if (progress != null)
                {
                    progress.Report(step++);
                }
            }
            _reset = false;
        }

        internal void SetWinner(int winner)
        {
            _hasWinner = winner;
            if (winner != 0)
            {
                _reset = true;
            }
        }
        internal void Reset(bool flag)
        {
            _reset = flag;
        }
        
    }

}
