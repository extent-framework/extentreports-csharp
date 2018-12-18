namespace AventStack.ExtentReports
{
    /// <summary>
    /// Marker interface for execution's result providing a Status
    /// </summary>
    public interface IRunResult
    {
        Status Status { get; }
    }
}
