namespace AventStack.ExtentReports.Core
{
    public interface IMediaContainer<T>
    {
        T AddScreenCaptureFromPath(string path, string title = null);
        T AddScreenCaptureFromBase64String(string s, string title = null);
    }
}
