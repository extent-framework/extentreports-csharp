namespace AventStack.ExtentReports
{
    /// <summary>
    /// Type of AnalysisStrategy for the reporter. 
    /// 
    /// Note: Not all reporters support this setting.
    /// 
    /// There are 3 types of strategies available:
    /// 
    /// <list type="bullet">
    ///     <item>BDD: Strategy for BDD-style (Gherkin) tests</item>
    ///     <item>Class: Shows analysis for 2 levels: Class, Test</item>
    ///     <item>Test: Used for 1 level only, but if a test has nodes, then it will automatically determine stats for Test and Methods</item>
    /// </list>
    /// </summary>
    public enum AnalysisStrategy
    {
        BDD,
        Class,
        Test
    }
}
