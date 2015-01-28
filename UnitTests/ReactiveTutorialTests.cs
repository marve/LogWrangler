using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using NSubstitute;

namespace UnitTests
{
    public class ReactiveTutorialTests
    {
        [Fact]
        public void MakeEmpty()
        {
            IObservable<int> observable = Observable.Create((IObserver<int> o) =>
            {
                o.OnCompleted();
                return Disposable.Empty;
            });
            IObserver<int> observer = Substitute.For<IObserver<int>>();
            observable.Subscribe(observer);
            observer.Received(0).OnNext(Arg.Any<int>());
            observer.Received(0).OnError(Arg.Any<Exception>());
            observer.Received(1).OnCompleted();
        }

        [Fact]
        public void MakeReturn()
        {
            int val = 0;
            IObservable<int> observable = Observable.Create((IObserver<int> o) =>
            {
                o.OnNext(val);
                o.OnCompleted();
                return Disposable.Empty;
            });
            IObserver<int> observer = Substitute.For<IObserver<int>>();
            observable.Subscribe(observer);
            observer.Received(1).OnNext(val);
            observer.Received(0).OnError(Arg.Any<Exception>());
            observer.Received(1).OnCompleted();
        }

        [Fact]
        public void MakeNever()
        {
            IObservable<int> observable = Observable.Create((IObserver<int> o) =>
            {
                return Disposable.Empty;
            });
            IObserver<int> observer = Substitute.For<IObserver<int>>();
            observable.Subscribe(observer);
            observer.Received(0).OnNext(Arg.Any<int>());
            observer.Received(0).OnError(Arg.Any<Exception>());
            observer.Received(0).OnCompleted();
        }

        [Fact]
        public void MakeThrow()
        {
            Exception e = new Exception("test");
            IObservable<int> observable = Observable.Create((IObserver<int> o) =>
            {
                o.OnError(e);
                o.OnCompleted();
                return Disposable.Empty;
            });
            IObserver<int> observer = Substitute.For<IObserver<int>>();
            observable.Subscribe(observer);
            observer.Received(0).OnNext(Arg.Any<int>());
            observer.Received(1).OnError(e);
            observer.Received(0).OnCompleted();
        }

        [Fact]
        public void MakeRange()
        {
            IObservable<int> observable = Observable.Generate(
                0,
                i => i < 10,
                i => i + 1,
                i => i);
            IObserver<int> observer = Substitute.For<IObserver<int>>();
            observable.Subscribe(observer);
            observer.Received(1).OnNext(0);
            observer.Received(1).OnNext(1);
            observer.Received(1).OnNext(2);
            observer.Received(1).OnNext(3);
            observer.Received(1).OnNext(4);
            observer.Received(1).OnNext(5);
            observer.Received(1).OnNext(6);
            observer.Received(1).OnNext(7);
            observer.Received(1).OnNext(8);
            observer.Received(1).OnNext(9);
            observer.Received(0).OnNext(Arg.Is<int>(i => i >= 10));
            observer.Received(0).OnNext(Arg.Is<int>(i => i < 0));
            observer.Received(1).OnCompleted();
            observer.Received(0).OnError(Arg.Any<Exception>());
        }
    }
}
