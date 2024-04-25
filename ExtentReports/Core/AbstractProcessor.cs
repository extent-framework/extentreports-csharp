using AventStack.ExtentReports.Model;
using AventStack.ExtentReports.Model.Convert;
using System.IO;

namespace AventStack.ExtentReports.Core
{
    public abstract class AbstractProcessor : ReactiveSubject
    {
        protected internal bool UsingNaturalConf = true; 
        
        private string[] _mediaResolverPath;

        protected internal void OnTestCreated(Test test)
        {
            Report.AddTest(test);
        }

        protected internal void OnTestRemoved(Test test)
        {
            Report.RemoveTest(test);
        }

        protected internal void OnNodeCreated(Test node)
        {
            
        }

        protected internal void OnLogCreated(Log log, Test test)
        {
            if (log.HasException)
            {
                Report.ExceptionInfoCtx.AddContext(log.ExceptionInfo, test);
            }
        }

        protected internal void OnMediaAdded(Media m, Test test)
        {
            TryResolveMediaPath(m);
        }

        protected internal void OnMediaAdded(Media m, Log log, Test test)
        {
            TryResolveMediaPath(m);
        }

        private void TryResolveMediaPath(Media media)
        {
            if (_mediaResolverPath == null || (media is ScreenCapture capture && capture.Base64 != null))
            {
                return;
            }
            
            if (!File.Exists(media.Path))
            {
                foreach (var p in _mediaResolverPath)
                {
                    var path = Path.Combine(p, media.Path);
                    
                    if (File.Exists(path))
                    {
                        media.ResolvedPath = path;
                        break;
                    }

                    var name = Path.GetFileName(media.Path);
                    path = Path.Combine(p, name);

                    if (File.Exists(path))
                    {
                        media.ResolvedPath = path;
                        break;
                    }
                }
            }
        }

        protected internal void TryResolveMediaPath(string[] path)
        {
            _mediaResolverPath = path;
        }

        protected internal void OnAuthorAdded(Author x, Test test)
        {
            Report.AuthorCtx.AddContext(x, test);
        }

        protected internal void OnCategoryAdded(Category x, Test test)
        {
            Report.CategoryCtx.AddContext(x, test);
        }

        protected internal void OnDeviceAdded(Device x, Test test)
        {
            Report.DeviceCtx.AddContext(x, test);
        }

        protected internal void OnFlush()
        {
            Report.Refresh();
            
            if (!UsingNaturalConf)
            {
                Report.ApplyOverrideConf();
            }
            
            Flush();
        }

        protected internal void OnReportLogAdded(string log)
        {
            Report.Logs.Enqueue(log);
        }

        protected internal void OnSystemInfoAdded(SystemEnvInfo env)
        {
            Report.SystemEnvInfo.Add(env);
        }

        protected internal void ConvertRawEntities(ExtentReports extent, string filePath)
        {
            var parser = new TestEntityParser(extent);
            parser.CreateEntities(filePath);
        }
    }
}
