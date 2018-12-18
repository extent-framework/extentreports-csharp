using System.Text;

namespace AventStack.ExtentReports.MarkupUtils
{
    internal class Table : IMarkup
    {
        public string[][] Data { get; set; }

        public string GetMarkup()
        {
            var sb = new StringBuilder();
            sb.Append("<table class='runtime-table table-striped table'>");
            for (int row = 0; row < Data.Length; row++)
            {
                sb.Append("<tr>");
                for (int col = 0; col < Data[row].Length; col++)
                {
                    sb.Append("<td>" + Data[row][col] + "</td>");
                }
                sb.Append("</tr>");
            }
            sb.Append("</table>");
            return sb.ToString();
        }
    }
}
