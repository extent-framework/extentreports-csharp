using System;

namespace AventStack.ExtentReports.Model
{
    [Serializable]
    public class ScreenCapture : Media
    {
        private const string Base64StringDataType = "data:image/png;base64,";

        public string Source
        {
            get
            {
                if (!string.IsNullOrEmpty(Base64String))
                {
                    return "<a href='" + Base64StringDataType + Base64String + "' data-featherlight='image'><span class='label grey badge white-text text-white'>base64-img</span></a>";
                }

                return "<img class='r-img' title='" + Title + "' onerror='this.style.display=\"none\"' data-featherlight='" + Path + "' src='" + Path + "' data-src='" + Path + "'>";
            }
        }

        public string SourceIcon
        {
            get
            {
                if (!string.IsNullOrEmpty(Base64String))
                {
                    return "<a href='" + Base64StringDataType + Base64String + "' data-featherlight='image'><span class='label grey badge white-text text-white'>base64-img</span></a>";
                }

                return "<a class='r-img' onerror='this.style.display=\"none\"' data-featherlight='" + Path + "' href='" + Path + "' data-src='" + Path + "'>" +
                        "<span class='label grey badge white-text text-white'>img</span>" +
                    "</a>";
            }
        }

        public string ScreenCapturePath
        {
            get
            {
                return !string.IsNullOrEmpty(base.Path) ? base.Path : Base64StringDataType + Base64String;
            }
        }

        public bool IsBase64
        {
            get
            {
                return !string.IsNullOrEmpty(Base64String);
            }
        }
    }
}
