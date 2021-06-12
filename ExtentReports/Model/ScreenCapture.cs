namespace AventStack.ExtentReports.Model
{
    public class ScreenCapture : Media
    {
        public ScreenCapture(string path = null, string title = null) : base(path, title) { }

        public string Base64 { get; set; }

        public bool IsBase64
        {
            get
            {
                return !string.IsNullOrEmpty(Base64);
            }
        }
    }
}
