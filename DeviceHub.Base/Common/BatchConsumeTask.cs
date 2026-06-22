using System;
using System.Collections.Generic;
using System.Text;
using DeviceHub.Base.Common;

namespace DeviceHub.Base.Common
{
    public interface IConsumeTask
    {
        void StartConsume();
        void NotifyConsume();
        void Shutdown();
    }

    public interface IBatchTaskHandler<T>
    {
        IEnumerable<T> SearchTask();
        void HandleTask(T task);
    }

    public class BatchConsumeTask<T> : IConsumeTask
    {
        private readonly IBatchTaskHandler<T> _taskHandler;
        private readonly Thread _thread;
        private readonly object _lockObj = new object();
        private readonly int _waitTimeoutMilliseconds;

        private volatile bool _isNewTask = false;
        private volatile bool _running = true;

        public BatchConsumeTask(IBatchTaskHandler<T> taskHandler)
            : this(taskHandler, "default", 60 * 1000)
        {
        }

        public BatchConsumeTask(IBatchTaskHandler<T> taskHandler, string handleType, int waitTimeoutMilliseconds)
        {
            _taskHandler = taskHandler;
            _waitTimeoutMilliseconds = waitTimeoutMilliseconds;

            _thread = new Thread(ConsumeLoop)
            {
                IsBackground = true,
                Name = $"{handleType}_batch_consume_task"
            };
        }

        public void StartConsume()
        {
            _thread.Start();
        }

        public void NotifyConsume()
        {
            lock (_lockObj)
            {
                _isNewTask = true;
                Monitor.Pulse(_lockObj);
            }
        }

        public void Shutdown()
        {
            _running = false;
            _thread.Interrupt();
        }

        private void ExecuteTask(T task)
        {
            try
            {
                _taskHandler.HandleTask(task);
            }
            catch (Exception ex)
            {
                Logger.Error($"batch consume handleTask error, thread:{Thread.CurrentThread.ManagedThreadId}, task:{task}", ex);
            }
        }

        private void ConsumeLoop()
        {
            //while (_running && !_thread.ThreadState.HasFlag(ThreadState.Stopped))
            while (_running)
            {
                _isNewTask = false;

                IEnumerable<T> tasks = null;

                try
                {
                    tasks = _taskHandler.SearchTask();
                }
                catch (Exception ex)
                {
                    Logger.Error("searchTask error", ex);
                }

                if (tasks == null)
                {
                    WaitForSignal();
                    continue;
                }

                bool hasAny = false;

                foreach (var task in tasks)
                {
                    hasAny = true;
                    ExecuteTask(task);
                }

                if (!hasAny)
                {
                    WaitForSignal();
                }
            }
        }

        private void WaitForSignal()
        {
            lock (_lockObj)
            {
                if (_isNewTask)
                {
                    return;
                }

                try
                {
                    Monitor.Wait(_lockObj, _waitTimeoutMilliseconds);
                }
                catch (ThreadInterruptedException)
                {
                    Logger.Warn($"batch consume thread interrupted: {_thread.Name}");
                }
            }
        }
    }
}
