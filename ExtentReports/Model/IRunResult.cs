namespace AventStack.ExtentReports.Model
{
    public interface IRunResult<in T>
    {
        Status Status { get; }
    }
}
