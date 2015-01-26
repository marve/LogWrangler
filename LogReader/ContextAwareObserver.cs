using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogReader
{
    /// <summary>
    /// A wrapper that surfaces observable notifications on a specified synchronization context.
    /// </summary>
    /// <typeparam name="T">The type to be observed.</typeparam>
    internal class ContextAwareObserver<T> : IObserver<T>
    {
        private readonly IObserver<T> _observer;
        private readonly SynchronizationContext _context;

        /// <summary>
        /// Creates a new <see cref="ContextAwareObserver"/>.
        /// </summary>
        /// <param name="observer">The underlying observer.</param>
        /// <param name="context">The context on which to observe.</param>
        internal ContextAwareObserver(IObserver<T> observer, SynchronizationContext context)
        {
            _observer = observer;
            _context = context;
        }

        public void OnCompleted()
        {
            _context.Post(_ => _observer.OnCompleted(), null);
        }

        public void OnError(Exception error)
        {
            _context.Post(e => _observer.OnError((Exception)e), error);
        }

        public void OnNext(T value)
        {
            _context.Post(t => _observer.OnNext((T)t), value);
        }
    }
}
