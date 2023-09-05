using System.Collections.Generic;
using System.Text;

namespace AventStack.ExtentReports.MarkupUtils
{
    internal class ListMarkup<T> : IMarkup
    {
        public string Type { get; internal set; }
        public IEnumerable<T> Object { get; internal set; }

        public string GetMarkup()
        {
            var sb = new StringBuilder();
            sb.Append("<" + Type + ">");

            foreach (var x in Object)
            {
                sb.Append("<li>");
                if (x != null)
                {
                    sb.Append(x.ToString());
                }
                sb.Append("</li>");
            }

            sb.Append("</" + Type + ">");
            return sb.ToString();
        }
    }
}
