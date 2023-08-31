using System;
using System.Collections.Generic;

namespace AventStack.ExtentReports.Model
{
    public class Log : IMetaDataStorable, IRunResult<Log>, IBaseEntity
    {
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public Status Status { get; set; } = Status.Pass;
        public string Details { get; set; }
        public int Seq { get; set; } = -1;
        public ExceptionInfo ExceptionInfo { get; set; }
        public ScreenCapture Media { get; set; }
        public IDictionary<string, object> Info { get; set; } = new Dictionary<string, object>();

        public bool HasMedia => Media != null;

        public bool HasException => ExceptionInfo != null;

        public void AddMedia(Media media)
        {
            if (media != null && ((media.Path != null || media.ResolvedPath != null)
                || media is ScreenCapture capture && capture.Base64 != null))
                Media = (ScreenCapture)media;
        }

        public Log(Status status = Status.Pass)
        {
            Status = status;
        }
    }
}
