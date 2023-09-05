using System.Collections.Concurrent;

namespace AventStack.ExtentReports.Extensions
{
    internal static class ConcurrentQueueExtensions
    {
        public static void Clear<T>(this ConcurrentQueue<T> queue)
        {
            while (!queue.IsEmpty)
            {
                _ = queue.TryDequeue(out _);
            }
        }
    }
}
