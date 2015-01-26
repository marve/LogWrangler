using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
namespace LogReader
{
    public interface ILogReader : IObservable<ILogEntry>
    {
        Task Start(string path, CancellationToken token);
        void Pause();
        void Resume();
    }
}
