using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AventStack.ExtentReports.MarkupUtils
{
    internal class Table<T> : IMarkup
    {
        public string[,] Data { get; internal set; }
        public object Object { get; internal set; }

        public string GetMarkup()
        {
            if (Object == null && Data == null)
            {
                return "";
            }

            var sb = new StringBuilder();
            if (Object != null)
            {
                sb.Append("<table class='table markup-table'>");
                sb.Append("<tr>");
                sb.Append("<thead>");

                var t = typeof(T);
                var props = t.GetProperties();

                foreach (var col in props)
                {
                    sb.Append("<th>");
                    sb.Append(col.Name);
                    sb.Append("</th>");
                }

                sb.Append("</tr>");
                sb.Append("</thead>");
                sb.Append("<tbody>");

                var tableInfo = new TableInfo();

                for (var ix = 0; ix < props.Length; ix++)
                {
                    var value = props[ix].GetValue(Object, null);
                    if (!(value is string) && (value is IEnumerable || value.GetType().IsArray))
                    {
                        foreach (var x in (IEnumerable)value)
                        {
                            if (value != null)
                            {
                                tableInfo.AddCellValue(x.ToString(), ix);
                            }
                            else
                            {
                                tableInfo.AddCellValue("", ix);
                            }
                        }
                    }
                    else
                    {
                        tableInfo.AddCellValue(value.ToString(), ix);
                    }
                }

                var maxRows = tableInfo.Cells.Max(x => x.Count);

                for (var row = 0; row < maxRows; row++)
                {
                    sb.Append("<tr>");
                    foreach (var cell in tableInfo.Cells)
                    {
                        var value = cell.Count <= row ? "" : cell[row].ToString();
                        sb.Append("<td>" + value + "</td>");
                    }
                    sb.Append("</tr>");
                }

                sb.Append("</tbody>");
                sb.Append("</table>");

                return sb.ToString();
            }

            sb.Append("<table class='table table-sm'>");
            for (int row = 0; row < Data.GetLength(0); row++)
            {
                sb.Append("<tr>");
                for (int col = 0; col < Data.GetLength(1); col++)
                {
                    sb.Append("<td>" + Data[row, col] + "</td>");
                }
                sb.Append("</tr>");
            }
            sb.Append("</table>");
            return sb.ToString();
        }

        class TableInfo
        {
            internal List<List<string>> Cells = new List<List<string>>();

            internal void AddCellValue(string value, int colIndex)
            {
                if (Cells.Count <= colIndex)
                {
                    Cells.Add(new List<string>());
                }

                Cells[colIndex].Add(value);
            }
        }
    }
}
