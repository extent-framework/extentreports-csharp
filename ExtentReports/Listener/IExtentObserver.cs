using AventStack.ExtentReports.Listener.Entity;

namespace AventStack.ExtentReports.Listener
{
    public interface IExtentObserver<T> where T : IObservedEntity
    {
    }
}
