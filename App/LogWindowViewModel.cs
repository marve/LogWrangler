using LogReader;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogWrangler.LogApp
{
    public class LogWindowViewModel : ViewModelBase, IObserver<ILogEntry>, IDisposable
    {
        private readonly ObservableCollection<ILogEntry> _logEntries;
        private IDisposable _unsubscriber;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public ObservableCollection<ILogEntry> LogEntries
        {
            get { return _logEntries; }
        }

        public string LogFile
        {
            get { return @"C:\workspace\LogWrangler\LogGenerator\bin\Debug\generated.log"; }
        }

        public LogWindowViewModel(ILogReader logReader)
        {
            _logEntries = new ObservableCollection<ILogEntry>();
            _unsubscriber = logReader.Subscribe(this);
            _cancellationTokenSource = new CancellationTokenSource();
            Task.Run(() => logReader.Start(LogFile, _cancellationTokenSource.Token));
        }

        public void Dispose()
        {
            if (_unsubscriber != null)
            {
                _unsubscriber.Dispose();
                _unsubscriber = null;
            }
        }

        public void OnCompleted()
        {
            
        }

        public void OnError(Exception error)
        {
            throw new Exception("An error occurred.", error);
        }

        public void OnNext(ILogEntry value)
        {
            _logEntries.Add(value);
        }
    }
}
