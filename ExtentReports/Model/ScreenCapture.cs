using System;

namespace AventStack.ExtentReports.Model
{
    [Serializable]
    public class ScreenCapture : Media
    {
        public string Source
        {
            get
            {
                if (!string.IsNullOrEmpty(Base64String))
                {
                    return "<a href='" + Base64String + "' data-featherlight='image'><span class='label grey badge white-text text-white'>base64-img</span></a>";
                }

                return "<img class='r-img' onerror='this.style.display=\"none\"' data-featherlight='" + Path + "' src='" + Path + "' data-src='" + Path + "'>";
            }
        }

        public string SourceIcon
        {
            get
            {
                if (!string.IsNullOrEmpty(Base64String))
                {
                    return "<a href='" + Base64String + "' data-featherlight='image'><span class='label grey badge white-text text-white'>base64-img</span></a>";
                }

                return "<a class='r-img' onerror='this.style.display=\"none\"' data-featherlight='" + Path + "' href='" + Path + "' data-src='" + Path + "'>" +
                        "<span class='label grey badge white-text text-white'>img</span>" +
                    "</a>";
            }
        }
    }
}
