namespace AventStack.ExtentReports
{
    public interface IExtentReporter : ITestListener, IAnalysisStrategyService
    {
        /// <summary>
        /// Name of the Reporter
        /// </summary>
        string ReporterName { get; }

        /// <summary>
        /// Starts passing run information to the reporter
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the reporter. Ensures no information is passed to the reporter.
        /// </summary>
        void Stop();

        /// <summary>
        /// Write to or update the target source (file, database)
        /// </summary>
        /// <param name="reportAggregates"><see cref="ReportAggregates"/></param>
        void Flush(ReportAggregates reportAggregates);
    }
}
