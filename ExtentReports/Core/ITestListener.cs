using AventStack.ExtentReports.Model;

namespace AventStack.ExtentReports.Core
{
    public interface ITestListener
    {
        void OnTestStarted(Test test);
        void OnTestRemoved(Test test);
        void OnNodeStarted(Test node);
        void OnLogAdded(Test test, Log log);
        void OnCategoryAssigned(Test test, Category category);
        void OnAuthorAssigned(Test test, Author author);
        void OnDeviceAssigned(Test test, Device device);
        void OnScreenCaptureAdded(Test test, ScreenCapture screenCapture);
        void OnScreenCaptureAdded(Log log, ScreenCapture screenCapture);
    }
}
