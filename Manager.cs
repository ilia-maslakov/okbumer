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

        public Manager(Form1 frm) {
            _mainForm = frm;
        }

        public void FoundWinner(int winner)
        {
            if (_mainJob != null) {
                _mainJob.SetWinner(winner);
            }
        }
        public async Task StartAsync()
        {
            var progress = new Progress<int>(v => _mainForm.UpdateProgress(v));
            await Start(progress);
        }

        private Task<bool> Start(IProgress<int> progress)
        {
            return Task.Run(() =>
            {
                _mainJob = new Job();
                _mainJob.Start(progress);
                return true;
            });
        }
    }

    class Job
    {
        private static int _hasWinner = 0;
        public void Start(IProgress<int> progress)
        {
            int step = 0;
            while (_hasWinner == 0)
            {
                Thread.Sleep(125);
                if (progress != null)
                {
                    progress.Report(step++);
                }
            }
        }

        internal void SetWinner(int winner)
        {
            _hasWinner = winner;
        }
    }

}
