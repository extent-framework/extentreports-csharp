using System.Collections.Generic;

namespace AventStack.ExtentReports.Model
{
    public interface IMetaDataStorable
    {
        IDictionary<string, object> Info { get; }
    }
}