using AventStack.ExtentReports.Model;

namespace AventStack.ExtentReports
{
    public class MediaEntityModelProvider
    {
        internal Media Media { get; private set; }

        internal MediaEntityModelProvider(Media media)
        {
            Media = media;
        }
    }
}
