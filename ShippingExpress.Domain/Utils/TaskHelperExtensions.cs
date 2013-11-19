using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShippingExpress.Domain.Utils
{
    public static class TaskHelperExtensions
    {
        public static Task<TOuterResult> Then<TInnerResult, TOuterResult>(this Task<TInnerResult> task,
            Func<TInnerResult, Task<TOuterResult>> continuation,
            CancellationToken cancellationToken = default(CancellationToken), bool runSynchronously=false,bool continueOnCapturedContext=true)
        {
            return task.ThenImpl(t => continuation(t.Result), cancellationToken, runSynchronously,
                continueOnCapturedContext);

            
        }

        static Task<TResult> ToTask<TResult>(this Task task,
            CancellationToken cancellationToken = default(CancellationToken), TResult result = default(TResult))
        {
            if (task == null)
                return null;
            if (task.IsCompleted)
            {
                if (task.IsFaulted)
                    return TaskHelpers.FromErrors<TResult>(task.Exception.InnerExceptions);
                if (task.IsCanceled || cancellationToken.IsCancellationRequested)
                    return TaskHelpers.Canceled<TResult>();
                if (task.Status==TaskStatus.RanToCompletion)
                {
                    return TaskHelpers.FromResult(result);
                }
            }
            return ToTaskContinuation(task, result);
        }

        private static Task<TResult> ToTaskContinuation<TResult>(Task task, TResult result)
        {
            var tcs = new TaskCompletionSource<TResult>();
            task.ContinueWith(innerTask =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                    tcs.TrySetResult(result);
                else
                    tcs.TrySetFromTask(innerTask);
            }, TaskContinuationOptions.ExecuteSynchronously);
            return tcs.Task;
        }

        static void TrySetFromTask<TResult>(this TaskCompletionSource<TResult> tcs, Task source)
        {
            if (source.Status == TaskStatus.Canceled)
            {
                tcs.TrySetCanceled();
                return;
            }
            if (source.Status == TaskStatus.Faulted)
            {
                tcs.TrySetException(source.Exception.InnerExceptions);
                return;
            }
            if (source.Status==TaskStatus.RanToCompletion)
            {
                var taskResult = source as Task<TResult>;
                tcs.TrySetResult(taskResult == null ? default(TResult) : taskResult.Result);
                return;
            }
            return;
        }
        static Task<TOuterResult> ThenImpl<TTask, TOuterResult>(this TTask task,
            Func<TTask, Task<TOuterResult>> continuation, CancellationToken cancellationToken, bool runSynchronously,
            bool continueOnCapturedContext) where TTask:Task
        {
            if (task.IsCompleted)
            {
                if (task.IsFaulted)
                    return TaskHelpers.FromErrors<TOuterResult>(task.Exception.InnerExceptions);
                if (task.IsCanceled || cancellationToken.IsCancellationRequested)
                    return TaskHelpers.Canceled<TOuterResult>();
                if (task.Status==TaskStatus.RanToCompletion)
                {
                    try
                    {
                        continuation(task);
                    }
                    catch (Exception e)
                    {
                        return TaskHelpers.FromError<TOuterResult>(e);
                    }
                }
            }
            return ThenImplContinuation(task, continuation, cancellationToken, runSynchronously,
                continueOnCapturedContext);
        }

        private static Task<TOuterResult> ThenImplContinuation<TOuterResult,TTask>(TTask task, Func<TTask, Task<TOuterResult>> continuation, CancellationToken cancellationToken, bool runSynchronously, bool continueOnCapturedContext) where TTask:Task
        {
            var syncCtxt = SynchronizationContext.Current;
            var tcs = new TaskCompletionSource<Task<TOuterResult>>();
            task.ContinueWith(innerTask =>
            {
                if (innerTask.IsFaulted)
                    tcs.TrySetException(innerTask.Exception.InnerExceptions);
                else if (innerTask.IsCanceled || continueOnCapturedContext)
                    tcs.TrySetCanceled();
                else
                {
                    if (syncCtxt != null && continueOnCapturedContext)
                    {
                        syncCtxt.Post(state =>
                        {
                            try
                            {
                                tcs.TrySetResult(continuation(task));
                            }
                            catch (Exception e)
                            {
                                tcs.TrySetException(e);
                            }
                        }, null);
                    }
                    else
                    {
                        tcs.TrySetResult(continuation(task));
                    }
                }
            }, runSynchronously ? TaskContinuationOptions.ExecuteSynchronously : TaskContinuationOptions.None);

            return tcs.Task.FastUnwrap();
        }

        internal static Task<TResult> FastUnwrap<TResult>(this Task<Task<TResult>> task)
        {
            Task<TResult> innerTask = task.Status == TaskStatus.RanToCompletion ? task.Result : null;
            return innerTask ?? task.Unwrap();
        }
    }

    internal struct AsyncVoid
    {
    }
}
