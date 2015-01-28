using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Humanizer;

namespace LogReader
{
    public class LogReader : ILogReader
    {
        private readonly ConcurrentDictionary<int, IObserver<ILogEntry>> _observers;
        private int _observerKeyCounter = 0;
        private bool _isPaused = false;

        public LogReader()
        {
            _observers = new ConcurrentDictionary<int, IObserver<ILogEntry>>();
        }

        public async Task Start(string path, CancellationToken token)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.Open,
                    FileAccess.Read, FileShare.ReadWrite, 0x1000, FileOptions.SequentialScan))
            using (StreamReader reader = new StreamReader(fileStream, Encoding.UTF8))
            {
                while (!token.IsCancellationRequested)
                {
                    if (!_isPaused && _observers.Any())
                    {
                        string line = await reader.ReadLineAsync();
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            LogEntry entry = new LogEntry { Text = line };
                            _observers.Values.AsParallel().ForAll(o => o.OnNext(entry));
                        }
                    }
                    await Task.Delay(1.Milliseconds());
                }
            }
        }

        public void Pause()
        {
            _isPaused = true;
        }

        public void Resume()
        {
            _isPaused = false;
        }

        public IDisposable Subscribe(IObserver<ILogEntry> observer)
        {
            int observerKey = _observerKeyCounter++;
            //SynchronizationContext context = SynchronizationContext.Current;
            //ContextAwareObserver<ILogEntry> contextObserver = new ContextAwareObserver<ILogEntry>(observer, context);
            if (!_observers.TryAdd(observerKey, observer))
            {
                observer.OnError(new InvalidOperationException("Count not add observer."));
            }
            return new Unsubscriber(observerKey, _observers);
        }

        private class Unsubscriber : IDisposable
        {
            private readonly int _key;
            private readonly ConcurrentDictionary<int, IObserver<ILogEntry>> _observers;

            public Unsubscriber(int key, ConcurrentDictionary<int, IObserver<ILogEntry>> observers)
            {
                _key = key;
                _observers = observers;
            }

            public void Dispose()
            {
                IObserver<ILogEntry> dummyEntry;
                if (!_observers.TryRemove(_key, out dummyEntry))
                {
                    
                }
            }
        }
    }
}
