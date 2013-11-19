using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ShippingExpress.Domain.Utils
{
    public static class TaskHelpers
    {
        public static Task<TResult> FromResult<TResult>(TResult result)
        {
            var tcs = new TaskCompletionSource<TResult>();
            tcs.SetResult(result);
            return tcs.Task;
        }

        public static Task<TResult> FromErrors<TResult>(ReadOnlyCollection<Exception> exceptions)
        {
            var tcs = new TaskCompletionSource<TResult>();
            tcs.SetException(exceptions);
            return tcs.Task;
        }

        public static Task<TResult> Canceled<TResult>()
        {
            var tcs = new TaskCompletionSource<TResult>();
            tcs.SetCanceled();
            return tcs.Task;
        }

        public static Task<TResult> FromError<TResult>(Exception exception)
        {
            var tcs = new TaskCompletionSource<TResult>();
            tcs.SetException(exception);
            return tcs.Task;
        }
    }
}
