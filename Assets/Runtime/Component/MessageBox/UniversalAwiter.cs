using System;
using System.Runtime.CompilerServices;

namespace Cat
{
    public class UniversalAwaiter<T> : INotifyCompletion
    {
        T result;
        Action continuation;
        public bool IsCompleted { get; set; }
        public UniversalAwaiter<T> GetAwaiter() => this;
        public void SetResult(T value)
        {
            this.result = value;
            IsCompleted = true;
            continuation?.Invoke();
        }
        public T GetResult() => this.result;
        public void OnCompleted(Action continuation)
        {
            this.continuation = continuation;
        }
    }
}