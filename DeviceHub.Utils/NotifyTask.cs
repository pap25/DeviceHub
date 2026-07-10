using System.Threading;

namespace DeviceHub.Utils
{
    public interface INotifyTaskHandler
    {
        void HandleTask();
    }

    public class NotifyTask : IConsumeTask
    {
        private readonly INotifyTaskHandler _taskHandler;
        private readonly Thread _thread;
        private readonly object _lockObj = new object();
        private readonly int _waitTimeoutMilliseconds;

        private volatile bool _isNewTask;

        public NotifyTask(INotifyTaskHandler taskHandler)
            : this(taskHandler, "default", 60 * 1000)
        {
        }

        public NotifyTask(INotifyTaskHandler taskHandler, string handleType, int waitTimeoutMilliseconds)
        {
            _taskHandler = taskHandler;
            _waitTimeoutMilliseconds = waitTimeoutMilliseconds;

            _thread = new Thread(ConsumeLoop)
            {
                IsBackground = true,
                Name = $"{handleType}_notify_task"
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
            _thread.Interrupt();
        }

        private void ConsumeLoop()
        {
            while (true)
            {
                _isNewTask = false;
                _taskHandler.HandleTask();

                lock (_lockObj)
                {
                    if (_isNewTask)
                    {
                        continue;
                    }

                    try
                    {
                        Monitor.Wait(_lockObj, _waitTimeoutMilliseconds);
                    }
                    catch (ThreadInterruptedException ex)
                    {
                        Logger.Info(nameof(NotifyTask),
                            $"interrupted notify task threadName:{Thread.CurrentThread.Name}, errorMsg:{ex.Message}");
                        break;
                    }
                }
            }
        }
    }
}
