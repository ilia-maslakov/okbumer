using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace okbumer
{
    class Manager : IDisposable
    {
        private static Form1 _mainForm;
        private static Job _mainJob;
        private static Task<bool> _mainTask;
        private CancellationTokenSource _cts;

        public Manager(Form1 frm)
        {
            _mainForm = frm;
            CreateCancelletionToken();
        }

        private void CreateCancelletionToken()
        {
            //_cts?.Dispose();
            _cts = new CancellationTokenSource();
        }

        public void Dispose()
        {
            _cts?.Dispose();
        }


        public void FoundWinner(int winner)
        {
            if (_mainJob != null) {
                if (winner != 0) {
                    _cts.Cancel();
                }
            }
        }

        public void Stop()
        {
            _cts.Cancel();
        }

        public void ResetJob()
        {
            if (_mainJob != null)
            {
                Stop();
            }
        }

        public async Task StartAsync()
        {
            CreateCancelletionToken();
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
                        _mainJob.Start(progress, _cts.Token);
                        return true;
                    });
            }
            return _mainTask;
        }
    }

    class Job
    {

        public async void Start(IProgress<int> progress, CancellationToken ct)
        {
            int step = 0;
            while (!ct.IsCancellationRequested)
            {
                try {
                    await Task.Delay(125, ct).ConfigureAwait(false);
                    if (progress != null)
                    {
                        progress.Report(step++);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    step = 0;
                    progress = null;
                }
            }
        }
    }

}
